using alpha;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    // ==================== Config Data
    [Header("[ Inventory Config ]")]
    [SerializeField] private SlotView m_inventorySlot;     // SlotPrefab
    [SerializeField] private SlotView m_equipSlot;
    [SerializeField] private SlotView m_quickSlot;
    
    [Space(10)]
    [SerializeField] private Transform m_scrollerView;

    public void CreateSlot(int count)
    {

    }

    public void CreateEquipSlot()
    {

    }

}
