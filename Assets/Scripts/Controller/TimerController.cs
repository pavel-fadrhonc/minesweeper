using System;
using Model;
using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View;

namespace DefaultNamespace
{
    public class TimerController : MonoBehaviour
    {
        private TimerView _timerView;
        private IGameStateModel _gameStateModel;

        private float _timer;

        private void Awake()
        {
            _timerView = GetComponent<TimerView>();
        }

        private void Start()
        {
            _gameStateModel = Locator.Instance.GameStateModel;
        }

        private void Update()
        {
            if (_gameStateModel.GameState == GameStateType.Running)
            {
                _timer += Time.deltaTime;
            
                UpdateTimerText();
            }
        }

        private void UpdateTimerText()
        {
            _timerView.SetTimerSeconds((int) Mathf.Floor(_timer));
        }
    }
}