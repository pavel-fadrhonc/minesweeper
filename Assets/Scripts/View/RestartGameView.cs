using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class RestartGameView : MonoBehaviour
    {
        public event Action RestartButtonClicked;
        
        private Button _restartGameButton;
        
        private void Awake()
        {
            _restartGameButton = GetComponentInChildren<Button>();
            
            _restartGameButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            RestartButtonClicked?.Invoke();
        }
    }
}