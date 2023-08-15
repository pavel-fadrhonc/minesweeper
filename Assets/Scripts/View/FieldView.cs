using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Model.Minefield;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup layout;

        public void AddCells(IReadOnlyList<IReadOnlyList<CellView>> cellViewRows)
        {
            int numRows = cellViewRows.Count;
            int numColumns = cellViewRows[0].Count;

            float cellSizeX = cellViewRows[0][0].GetComponent<RectTransform>().sizeDelta.x;
            float cellSizeY = cellViewRows[0][0].GetComponent<RectTransform>().sizeDelta.y;
            
            foreach (var cellViewRow in cellViewRows)
            {
                foreach (var cellView in cellViewRow)
                {
                    cellView.transform.SetParent(layout.transform);
                    cellView.transform.localScale = Vector3.one;
                }
            }

            layout.GetComponent<RectTransform>().sizeDelta = new Vector2(numColumns * cellSizeX,  numRows* cellSizeY);
            layout.cellSize = new Vector2(cellSizeX, cellSizeY);
        }
    }
}