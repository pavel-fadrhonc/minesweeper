using System;
using UnityEngine;

namespace DefaultNamespace.Model.Minefield
{
    [Serializable]
    public struct CellPosition : IEquatable<CellPosition>
    {
        [SerializeField]
        private uint x;
        [SerializeField]
        private uint y;

        public CellPosition(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }

        public uint X => x;
        public uint Y => y;

        public bool Equals(CellPosition other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is CellPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}