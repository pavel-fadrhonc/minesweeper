using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ZoomView : MonoBehaviour
    {
        [SerializeField]
        private GameObject controlObject;
        
        private Slider _slider;

        private const string ZOOM_LEVEL_KEY = "ZoomLevel";
        
        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();

            if (PlayerPrefs.HasKey(ZOOM_LEVEL_KEY))
            {
                var zoom = PlayerPrefs.GetFloat(ZOOM_LEVEL_KEY);
                controlObject.transform.localScale = new Vector3(zoom, zoom, 1);
                _slider.value = zoom;
            }
        }

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float val)
        {
            controlObject.transform.localScale = new Vector3(val, val, 1);
            
            PlayerPrefs.SetFloat(ZOOM_LEVEL_KEY, val);
        }
    }
}