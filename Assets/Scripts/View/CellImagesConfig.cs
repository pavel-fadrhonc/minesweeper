using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace View
{
    [CreateAssetMenu(fileName = nameof(CellImagesConfig), menuName = "Minesweeper/Cell images config")]
    public class CellImagesConfig : ScriptableObject
    {
        [SerializeField]
        private Sprite hiddenImage;
        [SerializeField]
        private Sprite markedMineImage;
        [SerializeField]
        private Sprite mineImage;
        [SerializeField]
        private Sprite emptyImage;
        [SerializeField]
        private Sprite _1mineImage;
        [SerializeField]
        private Sprite _2mineImage;
        [SerializeField]
        private Sprite _3mineImage;
        [SerializeField]
        private Sprite _4mineImage;
        [SerializeField]
        private Sprite _5mineImage;
        [SerializeField]
        private Sprite _6mineImage;
        [SerializeField]
        private Sprite _7mineImage;
        [SerializeField]
        private Sprite _8mineImage;
        [SerializeField]
        private Sprite _debugMineImage;
        [SerializeField]
        private Sprite _aiMoveImage;

        public Sprite HiddenImage => hiddenImage;
        public Sprite MarkedMineImage => markedMineImage;
        public Sprite EmptyImage => emptyImage;
        public Sprite MineImage => mineImage;
        public Sprite DebugMineImage => _debugMineImage;
        public Sprite AIMoveImage => _aiMoveImage;

        private Dictionary<uint, Sprite> _minesSprites;

        private void Init()
        {
            _minesSprites = new Dictionary<uint, Sprite>()
            {
                {1, _1mineImage},
                {2, _2mineImage},
                {3, _3mineImage},
                {4, _4mineImage},
                {5, _5mineImage},
                {6, _6mineImage},
                {7, _7mineImage},
                {8, _8mineImage},
            };
        }
        
        private void OnEnable()
        {
            Init();
        }

        public Sprite GetImageForNumMines(uint numMines)
        {
            if (_minesSprites.Count == 0)
                Init();

            if (!_minesSprites.ContainsKey(numMines))
                return null;

            return _minesSprites[numMines];
        }
    }
}