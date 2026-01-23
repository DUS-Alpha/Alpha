using UnityEngine;
using UnityEngine.EventSystems;

namespace alpha
{
    public class TriggerEvent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            WorldAudioManager.Instance.PlayHover();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            WorldAudioManager.Instance.PlayClick();
        }
    }
}