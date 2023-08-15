using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Service
{
    public class SetCountFieldDataProvider : IFieldDataProvider
    {
        private readonly Settings _settings;
        private List<Vector2Int> _fieldData = new List<Vector2Int>();
        
        public SetCountFieldDataProvider(Settings settings)
        {
            _settings = settings;
        }

        public IReadOnlyList<Vector2Int> GetMinePositions(uint fieldSizeX, uint fieldSizeY)
        {
            var minesCount = _settings.NumberOfMines;
            
            while (_fieldData.Count < minesCount)
            {
                int randomX;
                int randomY;
                bool alreadyPresent = false;

                do
                {
                    randomX = Random.Range(0, (int) fieldSizeX);
                    randomY = Random.Range(0, (int) fieldSizeY);

                    var pos = new Vector2Int(randomX, randomY);

                    alreadyPresent = _fieldData.Contains(pos);

                    if (!alreadyPresent)
                        _fieldData.Add(pos);

                } while (alreadyPresent);
            }

            return _fieldData;
        }

        [Serializable]
        public class Settings
        {
            public int NumberOfMines;
        }
    }
}