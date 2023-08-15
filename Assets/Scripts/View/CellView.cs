using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
    public class CellView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Image _foregroundImage;  
        [SerializeField]
        private Image _overlayImage;  
        
        public enum ClickType
        {
            RightClick,
            LeftClick
        }
        
        public event Action<ClickType> CellClickedEvent;

        public void SetBackgroundImage(Sprite image)
        {
            _backgroundImage.enabled = image != null;
            _backgroundImage.sprite = image;
        }

        public void SetForegroundImage(Sprite image)
        {
            _foregroundImage.enabled = image != null;
            _foregroundImage.sprite = image;
        }

        public void SetOverlayImage(Sprite image)
        {
            _overlayImage.enabled = image != null;
            _overlayImage.sprite = image;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                CellClickedEvent?.Invoke(ClickType.RightClick);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                CellClickedEvent?.Invoke(ClickType.LeftClick);
            }
        }

        public void SimulateClick(ClickType clickType)
        {
            CellClickedEvent?.Invoke(clickType);
        }
    }
}