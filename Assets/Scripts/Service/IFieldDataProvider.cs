using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public interface IFieldDataProvider
    {
        public IReadOnlyList<Vector2Int> GetMinePositions(uint fieldSizeX, uint fieldSizeY);
    }
}