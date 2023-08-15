using System;
using System.IO;
using System.Linq;
using DefaultNamespace.Model;
using DefaultNamespace.Model.Minefield;
using Model;
using Service;
using UnityEngine;
using View;

namespace DefaultNamespace
{
    public class MinefieldGenerator : MonoBehaviour
    {
        private FieldController _fieldController;
        private CellImagesConfig _cellImagesConfig;
        private CellView _cellPrefab;
        private IFieldDataProvider _fieldDataProvider;
        private IMineField _mineField;
        private IMineFieldViewData _mineFieldViewData;
        private IGameSettings _gameSettings;

        private void Start()
        {
            _fieldDataProvider = Locator.Instance.FieldDataProvider;
            _fieldController = Locator.Instance.FieldController;
            _cellImagesConfig = Locator.Instance.CellImagesConfig;
            _cellPrefab = Locator.Instance.CellPrefab;
            _mineField = Locator.Instance.MineField;
            _mineFieldViewData = Locator.Instance.MineFieldViewData;
            _gameSettings = Locator.Instance.GameSettings;
            
            Generate();
        }
        
        private void Generate()
        {
            uint fieldSizeX = _mineField.DimensionsXY.Item1;
            uint fieldSizeY = _mineField.DimensionsXY.Item2;
            
            var minePositions = _fieldDataProvider.GetMinePositions(fieldSizeX, fieldSizeY);

            if (_gameSettings.TryLoadBoardFromFile)
            {
                var jsonBoard = new JSONFileFieldDataProvider(Application.dataPath + $"/{_gameSettings.BoardFileName}");
                var boardData = jsonBoard.GetMinePositions();
                if (boardData != null)
                {
                    minePositions = jsonBoard.GetMinePositions().Select(cell => new Vector2Int((int) cell.X, (int) cell.Y)).ToList();
                }
            }
            
            for (int x = 0; x < fieldSizeX; x++)
            {
                for (int y = 0; y < fieldSizeY; y++)
                {
                    IMineField.CellType cellType = IMineField.CellType.Empty;
                    
                    if (minePositions.Contains(new Vector2Int(x, y)))
                        cellType = IMineField.CellType.Mine;

                    _mineField[(uint) x, (uint) y] = cellType;
                }
            }
            
            _fieldController.ConstructFrom(_mineField, _cellPrefab, _cellImagesConfig, _mineFieldViewData);
        }

        [Serializable]
        public class Settings
        {
            [SerializeField] private uint fieldSizeX;
            [SerializeField] private uint fieldSizeY;

            public uint FieldSizeX => fieldSizeX;
            public uint FieldSizeY => fieldSizeY;
        }
    }
}