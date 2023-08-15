using System.Collections.Generic;
using DefaultNamespace.Model.Minefield;
using Service;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    public static class CreateSchemaTest
    {
        //[MenuItem("MineSweeper/Create Json")]
        public static void Create()
        {
            IMineField mineField = Locator.Instance.MineField;
            
            var data = new JSONFileFieldDataProvider.JSONFieldData();
            var positions = new List<CellPosition>();

            var sizeX = mineField.DimensionsXY.Item1;
            var sizeY = mineField.DimensionsXY.Item2;

            for (uint x = 0; x < sizeX; x++)
            {
                for (uint y = 0; y < sizeY; y++)
                {
                    if (mineField[x, y] == IMineField.CellType.Mine)
                        positions.Add(new CellPosition(x, y));   
                }
            }

            data.MinePositions = positions;
            data.SizeX = sizeX;
            data.SizeY = sizeY;

            var json = JsonUtility.ToJson(data);
            
        }
    }
}