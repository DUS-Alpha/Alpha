using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace alpha
{
    public class ScrollRectNoDrag : ScrollRect
    {
        public bool IsBlockMouseDrag = false;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (IsBlockMouseDrag && eventData.pointerId == -1) // -1 = 마우스
                return;
            base.OnBeginDrag(eventData);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (IsBlockMouseDrag && eventData.pointerId == -1)
                return;
            base.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (IsBlockMouseDrag && eventData.pointerId == -1)
                return;
            base.OnEndDrag(eventData);
        }
    }
}