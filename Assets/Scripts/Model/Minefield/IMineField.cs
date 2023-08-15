using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Model.Minefield
{
    public interface IMineField
    {
        public (uint, uint) DimensionsXY { get; }
        
        CellType this[uint x, uint y] { get; set; }

        uint GetNumMinesSurrounding(uint x, uint y);

        public IReadOnlyList<CellPosition> GetMinePositions();

        IReadOnlyList<EmptyField> GetExpansionFrom(uint x, uint y);

        public enum CellType
        {
            Empty,
            Mine,
        }
    }
}