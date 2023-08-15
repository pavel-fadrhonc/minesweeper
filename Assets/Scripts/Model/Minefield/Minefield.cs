using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using View;

namespace DefaultNamespace.Model.Minefield
{
    public class Minefield : IMineField
    {
        private IMineField.CellType[,] _field;
        private uint _sizeX;
        private uint _sizeY;
        private List<CellPosition> _minesPositions = new List<CellPosition>();
        
        public Minefield(uint sizeX, uint sizeY)
        {
            _field = new IMineField.CellType[sizeX, sizeY];
            _sizeX = sizeX;
            _sizeY = sizeY;
        }

        // public CellType? GetCellType(uint posX, uint posY)
        // {
        //     if (posX > _sizeX || posY > _sizeY)
        //         return null;
        //
        //     return _field[posX, posY];
        // }

        public (uint, uint) DimensionsXY => (_sizeX, _sizeY);

        public IMineField.CellType this[uint x, uint y]
        {
            get
            {
                if (x >= _sizeX || y >= _sizeY)
                    throw new IndexOutOfRangeException(
                        $"Minefield has only [{_sizeX}{_sizeY} elements but was indexed with [{x}{y}]]");
                
                return _field[x, y];
            }
            set
            { 
                if (x >= _sizeX || y >= _sizeY)
                    throw new IndexOutOfRangeException(
                        $"Minefield has only [{_sizeX}{_sizeY} elements but was indexed with [{x}{y}]]");
                
                var data = _field[x, y];
                if (value == data)
                    return;

                _field[x, y] = value;

                if (data == IMineField.CellType.Mine)
                {
                    var minePosIndex = _minesPositions.IndexOf(new CellPosition(x, y));
                    if (minePosIndex > -1)
                        _minesPositions.RemoveAt(minePosIndex);
                }
                else if (value == IMineField.CellType.Mine)
                {
                    var minePosition = new CellPosition(x, y);
                    if (!_minesPositions.Contains(minePosition))
                        _minesPositions.Add(minePosition);
                }
            }
        }

        public uint GetNumMinesSurrounding(uint x, uint y)
        {
            var minX = (uint) Mathf.Max(0, x - 1);
            var minY = (uint) Mathf.Max(0, y - 1);
            var maxX = (uint) Mathf.Min(_sizeX - 1, x + 1);
            var maxY = (uint) Mathf.Min(_sizeY - 1, y + 1);

            uint numSurrounding = 0;
            
            for (uint xs = minX; xs <= maxX; xs++)
            {
                for (uint ys = minY; ys <= maxY; ys++)
                {
                    var cellType = this[xs, ys];

                    if (cellType == IMineField.CellType.Mine)
                        numSurrounding++;
                }
            }

            return numSurrounding;
        }

        public IReadOnlyList<CellPosition> GetMinePositions() => _minesPositions;
        /// <summary>
        /// Performs expansion on given position and returns all empty positions.
        /// Returned collection is invalidated by subsequent calls.
        /// </summary>
        /// <param name="x">X position on grid.</param>
        /// <param name="y">Y position on grid.</param>
        /// <returns>Collection of empty points connected to [x, y]. Null if [x, y] is mine.</returns>
        private List<EmptyField> _emptyFieldsSurrounding = new List<EmptyField>();
        public IReadOnlyList<EmptyField> GetExpansionFrom(uint x, uint y)
        {
            if (this[x, y] != IMineField.CellType.Empty)
                return null;
            
            _emptyFieldsSurrounding.Clear();

            ExpansionFrom(x, y, _emptyFieldsSurrounding);

            return _emptyFieldsSurrounding;
        }

        private void ExpansionFrom(uint x, uint y, List<EmptyField> collection)
        {
            if (x >= _sizeX || y >= _sizeY)
                return;

            if (this[x, y] != IMineField.CellType.Empty)
                return;

            var adjacentNumCount = GetNumMinesSurrounding(x, y);

            var field = new EmptyField() {cellPosition = new CellPosition(x, y), adjacentMinesNum = adjacentNumCount};

            if (collection.Contains(field))
                return;
            
            collection.Add(field);

            if (adjacentNumCount == 0)
            {
                ExpansionFrom(x - 1, y, collection);
                ExpansionFrom(x + 1, y, collection);
                ExpansionFrom(x, y - 1, collection);
                ExpansionFrom(x, y + 1, collection);
                ExpansionFrom(x + 1, y + 1, collection);
                ExpansionFrom(x + 1, y - 1, collection);
                ExpansionFrom(x - 1, y + 1, collection);
                ExpansionFrom(x - 1, y - 1, collection);
            }
        }
    }
}