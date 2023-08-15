using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Service
{
    public class RandomFieldDataProvider : IFieldDataProvider
    {
        private List<Vector2Int> _fieldData = new List<Vector2Int>();

        private readonly Settings _settings;
        
        public RandomFieldDataProvider(Settings settings)
        {
            _settings = settings;
        }
        
        public IReadOnlyList<Vector2Int> GetMinePositions(uint fieldSizeX, uint fieldSizeY)
        {
            _fieldData.Clear();

            var density = _settings.Density;
            
            for (int x = 0; x < fieldSizeX; x++)
            {
                for (int y = 0; y < fieldSizeY; y++)
                {
                    if (Random.Range(0f,1f) < density)
                        _fieldData.Add(new Vector2Int(x, y));
                }
            }

            return _fieldData;
        }

        [Serializable]
        public class Settings
        {
            [Tooltip("How much is the field populated with mines")]
            [Range(0f,1f)]
            [SerializeField] private float _density;

            public float Density => _density;
        }
    }
}