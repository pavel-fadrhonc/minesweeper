using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace.Model.Minefield;
using UnityEngine;

namespace Service
{
    public class JSONFileFieldDataProvider
    {
        public class JSONFieldData
        {
            public List<CellPosition> MinePositions;
            public uint SizeX;
            public uint SizeY;
        }

        public uint SizeX
        {
            get
            {
                if (_fieldData == null) return 0;

                return _fieldData.SizeX;
            }
        }
        
        public uint SizeY
        {
            get
            {
                if (_fieldData == null) return 0;

                return _fieldData.SizeY;
            }
        }

        private JSONFieldData _fieldData;

        public JSONFileFieldDataProvider(string fileName)
        {
            if (!File.Exists(fileName))
                return;
            
            var json = File.ReadAllText(fileName);
            _fieldData = JsonUtility.FromJson<JSONFieldData>(json);
        }
        
        public IReadOnlyList<CellPosition> GetMinePositions()
        {
            if (_fieldData == null ||
                _fieldData.MinePositions == null)
                return null;

            return _fieldData.MinePositions;
        }
    }
}