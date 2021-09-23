using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private Color32 textColor;
        [SerializeField] private Color32 textColorHover;
        
        private TextMeshProUGUI _text;
        private AudioSource _audioSource;

        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _audioSource = GetComponentInChildren<AudioSource>();
            _text.color = textColor;
        }

        // private void Update()
        // {
        //     _text.color =  textColor;
        // }

        public void OnPointerEnter(PointerEventData eventData)
        {
             _text.color =  textColorHover;
            _audioSource.Play();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _text.color = textColorHover;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _text.color = textColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = textColor;
        }
    }
}