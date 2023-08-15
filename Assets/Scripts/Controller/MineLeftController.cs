using DefaultNamespace.Events;
using DefaultNamespace.Model.Minefield;
using Service;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class MineLeftController : MonoBehaviour
    {
        private MinesLeftView _minesLeftView;

        private IMineField _mineField;
        private IMineFieldViewData _mineFieldViewData;
        
        private void Awake()
        {
            _minesLeftView = GetComponent<MinesLeftView>();
        }

        private void Start()
        {
            _mineField = Locator.Instance.MineField;
            _mineFieldViewData = Locator.Instance.MineFieldViewData;
            
            _mineFieldViewData.DataChangedEvent += OnMineFieldViewDataChangedEvent;
            
            RefreshMinesCount();
        }

        private void OnMineFieldViewDataChangedEvent(IMineFieldViewData.DataChangedInfo info)
        {
            if (info.NewCellType == IMineFieldViewData.CellViewType.Marked ||
                info.PreviousCellType == IMineFieldViewData.CellViewType.Marked)
            {
                RefreshMinesCount();    
            }
        }

        private void RefreshMinesCount()
        {
            var totalMinesCount = _mineField.GetMinePositions().Count;
            var markedMinesCount = _mineFieldViewData.GetNumMarked();

            var remainingMinesCount = totalMinesCount - markedMinesCount;
            
            _minesLeftView.SetMinesLeft(remainingMinesCount);
        }
    }
}