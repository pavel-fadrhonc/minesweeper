using System;

namespace DefaultNamespace.Model.Minefield
{
    public struct EmptyField : IEquatable<EmptyField>
    {
        public CellPosition cellPosition;
        public uint adjacentMinesNum;

        public bool Equals(EmptyField other)
        {
            return cellPosition.Equals(other.cellPosition) && adjacentMinesNum == other.adjacentMinesNum;
        }

        public override bool Equals(object obj)
        {
            return obj is EmptyField other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(cellPosition, adjacentMinesNum);
        }
    }
}