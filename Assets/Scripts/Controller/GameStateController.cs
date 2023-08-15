using DefaultNamespace.Events;
using DefaultNamespace.Model.Minefield;
using Model;
using Service;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameStateController : MonoBehaviour, IEventListener<MineFoundEvent>, IEventListener<PlayerWonEvent>
    {
        private IGameStateModel _gameStateModel;
        private IMineFieldViewData _mineFieldViewData;
        
        private void Start()
        {
            _gameStateModel = Locator.Instance.GameStateModel;
            _mineFieldViewData = Locator.Instance.MineFieldViewData;
            
            _mineFieldViewData.DataChangedEvent += OnMineFieldDataChangedEvent;

            _gameStateModel.GameState = GameStateType.NotStarted;
            
            EventDispatcher.AddListener<MineFoundEvent>(this);
            EventDispatcher.AddListener<PlayerWonEvent>(this);
        }

        private void OnMineFieldDataChangedEvent(IMineFieldViewData.DataChangedInfo info)
        {
            if (_gameStateModel.GameState == GameStateType.NotStarted)
            {
                _gameStateModel.GameState = GameStateType.Running;
            }
        }

        public void OnEvent(MineFoundEvent evt)
        {
            _gameStateModel.GameState = GameStateType.Finished;
        }

        public void OnEvent(PlayerWonEvent evt)
        {
            _gameStateModel.GameState = GameStateType.Finished;
        }
    }
}