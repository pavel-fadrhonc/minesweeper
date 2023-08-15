using System;

namespace Model
{
    public enum GameStateType
    {
        NotStarted,
        Running,
        Finished
    }
    
    public interface IGameStateModel
    {
        public event Action<GameStateType> GameStateChangedEvent;

        public GameStateType GameState { get; set; }
    }

    public class GameStateModel : IGameStateModel
    {
        public event Action<GameStateType> GameStateChangedEvent;

        private GameStateType _gameState;
        
        public GameStateType GameState
        {
            get => _gameState;
            set
            {
                if (_gameState == value)
                    return;

                _gameState = value;
                
                GameStateChangedEvent?.Invoke(_gameState);
            } 
        }
    }
}