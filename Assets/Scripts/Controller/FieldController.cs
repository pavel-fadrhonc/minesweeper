using System.Collections.Generic;
using DefaultNamespace.Model.Minefield;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class FieldController : MonoBehaviour
    {
        [SerializeField]
        private FieldView _fieldView;
        
        List<List<CellView>> _cellViews = new();
        
        public void ConstructFrom(
            IMineField mineField, 
            CellView cellPrefab, 
            CellImagesConfig cellImagesConfig,
            IMineFieldViewData mineFieldViewData)
        {
            var sizeX = mineField.DimensionsXY.Item1;
            var sizeY = mineField.DimensionsXY.Item2;
            
            for (int y = 0; y < sizeY; y++)
            {
                var currentCellRow = new List<CellView>();
                _cellViews.Add(currentCellRow);
                for (int x = 0; x < sizeX; x++)
                {
                    var cellInstance = Instantiate(cellPrefab);
                    var cellController = cellInstance.GetComponent<CellController>();
                    cellController.FulfillDependencies(mineField, (uint) x, (uint) y, cellImagesConfig, mineFieldViewData);
                
                    currentCellRow.Add(cellInstance);
                }
            }
            
            _fieldView.AddCells(_cellViews);
        }

        public CellView GetCellViewAt(uint posX, uint posY)
        {
            if (_cellViews.Count <= posY || _cellViews[0].Count <= posX)
                return null;

            return _cellViews[(int) posY][(int) posX];
        }
    }
}