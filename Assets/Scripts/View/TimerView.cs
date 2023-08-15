using TMPro;
using UnityEngine;

namespace View
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timeText;

        private void Awake()
        {
            timeText.text = "000";
        }

        public void SetTimerSeconds(int seconds)
        {
            timeText.text = seconds.ToString("000");
        }
    }
}