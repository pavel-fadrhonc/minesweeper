using UnityEngine;

namespace Model
{
    [CreateAssetMenu(menuName = "Minesweeper/Game Settings", fileName = nameof(GameSettings))]
    public class GameSettings : ScriptableObject, IGameSettings
    {
        [SerializeField]
        private bool _debugView;
        [SerializeField]
        private bool _tryLoadBoardFromFile;
        [SerializeField] 
        private string _boardFileName;

        public bool DebugView => _debugView;
        public bool TryLoadBoardFromFile => _tryLoadBoardFromFile;
        public string BoardFileName => _boardFileName;
    }
}