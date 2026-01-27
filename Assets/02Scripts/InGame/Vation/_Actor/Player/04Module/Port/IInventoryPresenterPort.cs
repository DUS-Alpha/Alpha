using System;

namespace alpha
{
    public interface IInventoryPresenterPort
    {
        event Action<int> OnCreateInventorySlotView;
    }
}