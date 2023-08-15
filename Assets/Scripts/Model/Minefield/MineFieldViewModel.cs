using System;

namespace DefaultNamespace.Model.Minefield
{
    public interface IMineFieldViewData
    {
        public event Action<IMineFieldViewData.DataChangedInfo> DataChangedEvent;
        
        CellViewType this[uint x, uint y] { get; set; }

        public int GetNumMarked();

        public int GetNumHidden();
        
        public struct DataChangedInfo
        {
            public uint PosX;
            public uint PosY;
            public CellViewType NewCellType;
            public CellViewType PreviousCellType;
        }
        
        public enum CellViewType
        {
            Revealed,
            Hidden,
            Marked
        }
    }
    
    public class MineFieldViewData : IMineFieldViewData
    {
        public event Action<IMineFieldViewData.DataChangedInfo> DataChangedEvent;

        private IMineField _mineField;
        private uint _sizeX;
        private uint _sizeY;

        private IMineFieldViewData.CellViewType[,] _fieldViewData;
        
        public MineFieldViewData(IMineField mineField)
        {
            _mineField = mineField;

            _sizeX = _mineField.DimensionsXY.Item1;
            _sizeY = _mineField.DimensionsXY.Item2;

            _fieldViewData = new IMineFieldViewData.CellViewType[_sizeX, _sizeY];

            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    _fieldViewData[x, y] = IMineFieldViewData.CellViewType.Hidden;
                }
            }
        }
        
        public IMineFieldViewData.CellViewType this[uint x, uint y]
        {
            get
            {
                if (x >= _sizeX || y >= _sizeY)
                    throw new IndexOutOfRangeException(
                        $"MinefieldViewData has only [{_sizeX}{_sizeY} elements but was indexed with [{x}{y}]]");
                
                return _fieldViewData[x, y];
            }
            set
            { 
                if (x >= _sizeX || y >= _sizeY)
                    throw new IndexOutOfRangeException(
                        $"MinefieldViewData has only [{_sizeX}{_sizeY} elements but was indexed with [{x}{y}]]");
                
                var data = _fieldViewData[x, y];
                if (value == data)
                    return;

                _fieldViewData[x, y] = value;
                
                DataChangedEvent?.Invoke(new IMineFieldViewData.DataChangedInfo() {PosX = x, PosY = y, NewCellType = value, PreviousCellType = data});
            }
        }

        public int GetNumMarked()
        {
            int count = 0;
            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    if (_fieldViewData[x, y] == IMineFieldViewData.CellViewType.Marked)
                        count++;
                }
            }

            return count;
        }

        public int GetNumHidden()
        {
            int count = 0;
            for (int x = 0; x < _sizeX; x++)
            {
                for (int y = 0; y < _sizeY; y++)
                {
                    if (_fieldViewData[x, y] == IMineFieldViewData.CellViewType.Hidden)
                        count++;
                }
            }

            return count;
        }
    }
}