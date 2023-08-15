using DefaultNamespace.Events;
using DefaultNamespace.Model;
using DefaultNamespace.Model.Minefield;
using Model;
using Service;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class CellController : MonoBehaviour
    {
        private CellView _cellView;

        private IMineField _mineField;
        private IMineFieldViewData _mineFieldViewData;
        private CellImagesConfig _cellImagesConfig;
        private IGameStateModel _gameStateModel;
        private IGameSettings _gameSettings;

        private uint PosX { get; set; }
        private uint PosY { get; set; }

        private void Awake()
        {
            _cellView = GetComponent<CellView>();
        }

        private void Start()
        {
            _cellView.CellClickedEvent += OnCellClickedEvent;
            _mineFieldViewData.DataChangedEvent += OnMineFieldDataChangedEvent;

            _gameStateModel = Locator.Instance.GameStateModel;
            _gameSettings = Locator.Instance.GameSettings;
            
            _cellView.SetOverlayImage(null);
            _cellView.SetBackgroundImage(_cellImagesConfig.HiddenImage);
            _cellView.SetForegroundImage(null);
            
            if (_gameSettings.DebugView &&
                _mineField[PosX, PosY] == IMineField.CellType.Mine)
            {
                _cellView.SetBackgroundImage(_cellImagesConfig.HiddenImage);
                _cellView.SetForegroundImage(_cellImagesConfig.DebugMineImage);
            }
        }
        
        public void FulfillDependencies(
            IMineField mineField, 
            uint posX, 
            uint posY,
            CellImagesConfig cellImagesConfig,
            IMineFieldViewData mineFieldViewData)
        {
            _mineField = mineField;
            PosX = posX;
            PosY = posY;
            _cellImagesConfig = cellImagesConfig;
            _mineFieldViewData = mineFieldViewData;
        }

        private void OnMineFieldDataChangedEvent(IMineFieldViewData.DataChangedInfo info)
        {
            if (info.PosX != PosX || info.PosY != PosY)
                return;
            
            switch (info.NewCellType)
            {
                case IMineFieldViewData.CellViewType.Hidden:
                    _cellView.SetBackgroundImage(_cellImagesConfig.HiddenImage);
                    _cellView.SetForegroundImage(null);
                    break;
                case IMineFieldViewData.CellViewType.Marked:
                    _cellView.SetBackgroundImage(_cellImagesConfig.HiddenImage);
                    _cellView.SetForegroundImage(_cellImagesConfig.MarkedMineImage);
                    break;
                case IMineFieldViewData.CellViewType.Revealed:
                    if (_mineField[PosX, PosY] == IMineField.CellType.Mine)
                    {
                        _cellView.SetBackgroundImage(_cellImagesConfig.EmptyImage);
                        _cellView.SetForegroundImage(_cellImagesConfig.MineImage);
                    }
                    else
                    {
                        var numSurrounding = _mineField.GetNumMinesSurrounding(PosX, PosY);
                    
                        _cellView.SetBackgroundImage(_cellImagesConfig.EmptyImage);
                    
                        if (numSurrounding == 0)
                        {
                            _cellView.SetForegroundImage(null);
                        }
                        else
                        {
                            _cellView.SetForegroundImage(_cellImagesConfig.GetImageForNumMines(numSurrounding));
                        }
                    }
                    
                    break;
            }
        }

        private void OnCellClickedEvent(CellView.ClickType clickType)
        {
            if (_gameStateModel.GameState == GameStateType.Finished)
                return;
            
            var cellViewType = _mineFieldViewData[PosX, PosY];
            
            if (clickType == CellView.ClickType.LeftClick)
            {
                if (cellViewType == IMineFieldViewData.CellViewType.Revealed)
                    return;

                var cellType = _mineField[PosX, PosY];

                if (cellType == IMineField.CellType.Empty)
                {
                    // get all empty fields in surroundings and perform expansion
                    var expansionFrom = _mineField.GetExpansionFrom(PosX, PosY);
                    foreach (var emptyField in expansionFrom)
                    {
                        _mineFieldViewData[emptyField.cellPosition.X, emptyField.cellPosition.Y] =
                            IMineFieldViewData.CellViewType.Revealed;
                    }
                }
                if (cellType == IMineField.CellType.Mine)
                {
                    EventDispatcher.Dispatch(new MineFoundEvent() { CellPosition = new CellPosition(PosX, PosY)});
                }
            }
            else
            {
                if (cellViewType == IMineFieldViewData.CellViewType.Revealed)
                    return;

                if (cellViewType == IMineFieldViewData.CellViewType.Marked)
                    _mineFieldViewData[PosX, PosY] = IMineFieldViewData.CellViewType.Hidden;
                else
                    _mineFieldViewData[PosX, PosY] = IMineFieldViewData.CellViewType.Marked;
            }
        }
    }
}