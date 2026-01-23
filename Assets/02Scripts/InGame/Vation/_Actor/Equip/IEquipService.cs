using alpha;
using UnityEngine;

namespace alpha
{
    public interface IEquipService
    {
        public void TryEquip(ItemSO data);
        public void TryUnEquip(ItemSO data);
        public bool CanSwap(int swapNum);
        public ItemModuleBase TrySwap(int swapNum);
    }
}