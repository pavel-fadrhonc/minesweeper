using System.Collections.Generic;
using DefaultNamespace.Events;
using DefaultNamespace.Model.Minefield;
using Service;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace DefaultNamespace
{
    public enum AiCellType
    {
        Hidden,
        Marked,
        Revealed
    }
    
    public class AiCell
    {
        public AiCellType AiCellType;
        public uint NeighbourMines;
    }

    public class AiPlayer : MonoBehaviour, IEventListener<PlayerWonEvent>, IEventListener<PlayerLostEvent>
    {
        [SerializeField]
        [Tooltip("How many moves per seconds does Ai do.")]
        private float moveFrequency = 1f;

        private float _movePeriod;
        private float _timeSinceLastMove;
        private uint _fieldSizeX;
        private uint _fieldSizeY;
        private List<List<AiCell>> _aiField = new ();
        private List<IAiMove> _aiMoves = new();
        private CellView _lastMoveCell;

        private CellImagesConfig _cellImagesConfig;
        private IMineField _mineField;
        private IMineFieldViewData _mineFieldViewData;
        private FieldController _fieldController;
        
        private void Start()
        {
            _cellImagesConfig = Locator.Instance.CellImagesConfig;
            _mineField = Locator.Instance.MineField;
            _mineFieldViewData = Locator.Instance.MineFieldViewData;
            _fieldController = Locator.Instance.FieldController;
            
            EventDispatcher.AddListener<PlayerWonEvent>(this);
            EventDispatcher.AddListener<PlayerLostEvent>(this);
            
            _movePeriod = 1f / moveFrequency;
            
            _mineFieldViewData.DataChangedEvent += OnViewDataChangedEvent;

            _fieldSizeX = _mineField.DimensionsXY.Item1;
            _fieldSizeY = _mineField.DimensionsXY.Item2;

            for (int x = 0; x < _fieldSizeX; x++)
            {
                _aiField.Add(new List<AiCell>());
                for (int y = 0; y < _fieldSizeY; y++)
                {
                    _aiField[x].Add(new AiCell() { AiCellType = AiCellType.Hidden});
                }
            }
        }

        private void OnViewDataChangedEvent(IMineFieldViewData.DataChangedInfo info)
        {
            if (info.NewCellType == IMineFieldViewData.CellViewType.Revealed)
            {
                // firs remove ai cell that were on this position
                for (var index = 0; index < _aiMoves.Count; index++)
                {
                    var aiMove = _aiMoves[index];
                    if (aiMove.PosX == info.PosX && aiMove.PosY == info.PosY)
                    {
                        var aiCell = GetCellAt(info.PosX, info.PosY);
                        if (aiCell.AiCellType != AiCellType.Revealed)
                        {
                            _aiMoves.RemoveAt(index);
                            break;
                        }
                    }
                }                
                
                var cell = GetCellAt(info.PosX, info.PosY);
                cell.AiCellType = AiCellType.Revealed;
                cell.NeighbourMines = _mineField.GetNumMinesSurrounding(info.PosX, info.PosY);
            }
            else if (info.NewCellType == IMineFieldViewData.CellViewType.Marked)
            {
                var cell = GetCellAt(info.PosX, info.PosY);
                cell.AiCellType = AiCellType.Marked;
            }
        }

        private void Update()
        {
            _timeSinceLastMove += Time.deltaTime;
            if (_timeSinceLastMove > _movePeriod)
            {
                if (_aiMoves.Count == 0)
                    CalculateMoves();

                if (_aiMoves.Count == 0)
                {
                    Debug.Log("<color=\"red\">Ran out of all moves!<color>");
                }
                else
                {
                    _aiMoves[0].DoMove();
                    _aiMoves.RemoveAt(0);
                }

                _timeSinceLastMove -= _movePeriod;
            }
        }

        private void CalculateMoves()
        {
            // first process and find if there are any cells that result in neighbor mark
            DoNeighborMark();
            DoNeighborReveal();
            if (_aiMoves.Count == 0)
                RevealRandomCell();
        }

        private void DoNeighborMark()
        {
            for (uint x = 0; x < _fieldSizeX; x++)
            {
                for (uint y = 0; y < _fieldSizeY; y++)
                {
                    var cell = GetCellAt(x, y);

                    var neighbourMines = cell.NeighbourMines;
                    
                    if (cell.AiCellType == AiCellType.Revealed && neighbourMines > 0)
                    { // run neighbour scan to find out if we can safely mark another cell
                        var hiddenNeighbourCount = 0;

                        for (uint xs = x - 1; xs <= x + 1; xs++)
                        {
                            for (uint ys = y - 1; ys <= y + 1; ys++)
                            {
                                var cellScanned = GetCellAt(xs, ys);
                                if (cellScanned == null)
                                    continue;;
                                
                                if (cellScanned.AiCellType == AiCellType.Hidden)
                                    hiddenNeighbourCount++;
                                else if (cellScanned.AiCellType == AiCellType.Marked)
                                    neighbourMines--;
                            }
                        }

                        if (hiddenNeighbourCount == neighbourMines)
                        { // mark all mines 
                            for (uint xs = x - 1; xs <= x + 1; xs++)
                            {
                                for (uint ys = y - 1; ys <= y + 1; ys++)
                                {
                                    var cellScanned = GetCellAt(xs, ys);
                                    if (cellScanned == null)
                                        continue;;

                                    if (cellScanned.AiCellType == AiCellType.Hidden)
                                    {
                                        var xsCopy = xs;
                                        var ysCopy = ys;
                                        AddMove(new AiMoveBase(xs, ys, () => MarkCellAsMine(xsCopy, ysCopy), $"Marking [{xs},{ys}] because [{x},{y}] has all remaining mines marked"));
                                    }
                                }
                            }                            
                        }
                    }
                }
            }
        }

        private void DoNeighborReveal()
        {
            for (uint x = 0; x < _fieldSizeX; x++)
            {
                for (uint y = 0; y < _fieldSizeY; y++)
                {
                    var cell = GetCellAt(x, y);

                    var neighbourMines = cell.NeighbourMines;
                    
                    if (cell.AiCellType == AiCellType.Revealed && neighbourMines > 0)
                    { // run neighbour scan to find out if we can safely mark another cell
                        var markedNeighbourCount = 0;

                        for (uint xs = x - 1; xs <= x + 1; xs++)
                        {
                            for (uint ys = y - 1; ys <= y + 1; ys++)
                            {
                                var cellScanned = GetCellAt(xs, ys);
                                if (cellScanned == null)
                                    continue;;

                                if (cellScanned.AiCellType == AiCellType.Marked)
                                    markedNeighbourCount++;
                            }
                        }

                        if (markedNeighbourCount == neighbourMines)
                        { // reveal all mines 
                            for (uint xs = x - 1; xs <= x + 1; xs++)
                            {
                                for (uint ys = y - 1; ys <= y + 1; ys++)
                                {
                                    var cellScanned = GetCellAt(xs, ys);
                                    if (cellScanned == null)
                                        continue;;

                                    if (cellScanned.AiCellType == AiCellType.Hidden)
                                    {
                                        var xsCopy = xs;
                                        var ysCopy = ys;
                                        AddMove(new AiMoveBase(xs, ys, () => RevealCell(xsCopy, ysCopy), $"Revealing [{xs},{ys}] because [{x},{y}] has all mines marked"));
                                    }
                                }
                            }                            
                        }
                    }
                }
            }
        }

        private List<(uint, uint)> _aiCellPool = new();
        private void RevealRandomCell()
        {
            _aiCellPool.Clear();
            
            for (uint x = 0; x < _fieldSizeX; x++)
            {
                for (uint y = 0; y < _fieldSizeY; y++)
                {
                    var cell = GetCellAt(x, y);
                    
                    if (cell.AiCellType == AiCellType.Hidden)
                        _aiCellPool.Add((x, y));
                }
            }

            var randomCell = _aiCellPool[Random.Range(0, _aiCellPool.Count)];

            var randomX = randomCell.Item1;
            var randomY = randomCell.Item2;
            
            AddMove(new AiMoveBase(randomX, randomY, () => RevealCell(randomX, randomY), $"Revealing [{randomX},{randomY}] at random because no clear move was available."));
        }

        private void RevealCell(uint xPos, uint yPos)
        {
            var aiCell = GetCellAt(xPos, yPos);
            aiCell.AiCellType = AiCellType.Revealed;
            
            var cellView = _fieldController.GetCellViewAt(xPos, yPos);
            
            cellView.SimulateClick(CellView.ClickType.LeftClick);

            if (_lastMoveCell != null)
            {
                _lastMoveCell.SetOverlayImage(null);
            }
            
            _lastMoveCell = cellView;
            _lastMoveCell.SetOverlayImage(_cellImagesConfig.AIMoveImage);
        }
        
        private void MarkCellAsMine(uint xPos, uint yPos)
        {
            var cellView = _fieldController.GetCellViewAt(xPos, yPos);

            cellView.SimulateClick(CellView.ClickType.RightClick);
            
            if (_lastMoveCell != null)
            {
                _lastMoveCell.SetOverlayImage(null);
            }

            _lastMoveCell = cellView;
            _lastMoveCell.SetOverlayImage(_cellImagesConfig.AIMoveImage);
        }

        private void AddMove(IAiMove aiMove)
        {
            var same = _aiMoves.Find(move => move.PosX == aiMove.PosX && move.PosY == aiMove.PosY);
            if (same != null)
                return;
            
            _aiMoves.Add(aiMove);
        }
        
        private AiCell GetCellAt(uint xPos, uint yPos)
        {
            if (xPos >= _fieldSizeX || yPos >= _fieldSizeY)
                return null;
            
            return _aiField[(int) xPos][(int) yPos];
        }
        
        public void OnEvent(PlayerWonEvent evt)
        {
            this.enabled = false;
        }
        
        public void OnEvent(PlayerLostEvent evt)
        {
            this.enabled = false;        
        }

        private void OnDestroy()
        {
            EventDispatcher.RemoveListener<PlayerWonEvent>(this);
            EventDispatcher.RemoveListener<PlayerLostEvent>(this);
        }
    }
}