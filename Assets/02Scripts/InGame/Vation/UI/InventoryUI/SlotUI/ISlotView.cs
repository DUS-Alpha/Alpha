using UnityEngine;

namespace alpha
{
    public interface ISlotView 
    {
        void SetIcon(Sprite icon);
        void SetCount(int count);
        void Clear();
    }
}