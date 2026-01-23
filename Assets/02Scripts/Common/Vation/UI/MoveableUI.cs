using UnityEngine;
using UnityEngine.EventSystems;

namespace alpha
{
    public class MoveableUI : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private RectTransform m_targetRt;

        private Vector2 m_movePoint;
        public void OnPointerDown(PointerEventData eventData)
        {
            m_movePoint = eventData.position;
        }
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 _pos = eventData.position - m_movePoint;
            m_movePoint = eventData.position;
            m_targetRt.anchoredPosition += _pos;
        }
    }
}