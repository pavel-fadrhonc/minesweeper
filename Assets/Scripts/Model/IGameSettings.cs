namespace Model
{
    public interface IGameSettings
    {
        public bool DebugView { get; }
        public bool TryLoadBoardFromFile { get; }
        public string BoardFileName { get; }
    }
}