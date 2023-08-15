using DefaultNamespace.Events;
using DefaultNamespace.Model.Minefield;
using Service;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameResultController : MonoBehaviour, IEventListener<MineFoundEvent>
    {
        [SerializeField]
        private GameObject _victoryGo;

        private IMineField _mineField;
        private IMineFieldViewData _mineFieldViewData;
        
        private void Start()
        {
            _mineField = Locator.Instance.MineField;
            _mineFieldViewData = Locator.Instance.MineFieldViewData;
            
            _mineFieldViewData.DataChangedEvent += OnMineFieldViewDataChangedEvent;
            
            EventDispatcher.AddListener(this);
        }

        private void OnMineFieldViewDataChangedEvent(IMineFieldViewData.DataChangedInfo info)
        {
            if (_mineFieldViewData.GetNumMarked() == _mineField.GetMinePositions().Count 
                && _mineFieldViewData.GetNumHidden() == 0)
            {
                uint sizeX = _mineField.DimensionsXY.Item1;
                uint sizeY = _mineField.DimensionsXY.Item2;

                for (uint x = 0; x < sizeX; x++)
                {
                    for (uint y = 0; y < sizeY; y++)
                    {
                        if (_mineField[x, y] == IMineField.CellType.Mine &&
                            _mineFieldViewData[x, y] != IMineFieldViewData.CellViewType.Marked)
                            return;
                    }
                }
                
                // VICTORY!!!
                _victoryGo.SetActive(true);
                EventDispatcher.Dispatch(new PlayerWonEvent());
            }
        }

        public void OnEvent(MineFoundEvent evt)
        {
            var minePositions = _mineField.GetMinePositions();
            foreach (var minePosition in minePositions)
            {
                _mineFieldViewData[minePosition.X, minePosition.Y] = IMineFieldViewData.CellViewType.Revealed;
            }
            
            EventDispatcher.Dispatch(new PlayerWonEvent());
        }
        
        private void OnDestroy()
        {
            EventDispatcher.RemoveListener(this);
        }
    }
}