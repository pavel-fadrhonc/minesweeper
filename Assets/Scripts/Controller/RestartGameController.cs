using UnityEngine;
using UnityEngine.SceneManagement;
using View;

namespace DefaultNamespace
{
    public class RestartGameController : MonoBehaviour
    {
        private RestartGameView _restartGameView;
        
        private void Awake()
        {
            _restartGameView = GetComponent<RestartGameView>();
            _restartGameView.RestartButtonClicked += OnRestartButtonClicked;
        }

        private void OnRestartButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}