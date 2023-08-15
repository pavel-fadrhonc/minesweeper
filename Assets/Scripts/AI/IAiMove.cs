namespace DefaultNamespace
{
    public interface IAiMove
    {
        uint PosX { get; }
        uint PosY { get; }
        
        void DoMove();
    }
}