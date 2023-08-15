using TMPro;
using UnityEngine;

namespace View
{
    public class MinesLeftView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI minesLeftText;

        public void SetMinesLeft(int minesLeft)
        {
            minesLeftText.text = minesLeft.ToString("000");
        }
    }
}