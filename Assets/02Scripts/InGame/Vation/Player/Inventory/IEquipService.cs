using alpha;
using UnityEngine;

namespace alpha
{
    public interface IEquipService
    {
        public void TryEquip(ItemDataSO data);
        public void TryUnEquip(ItemDataSO data);
        public bool CanSwap(int swapNum);
        public Item TrySwap(int swapNum);
    }
}