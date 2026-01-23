using UnityEngine;

public class InventoryViewManager : MonoBehaviour
{
    [Header("[ Ref Config ]")]
    // Slot
    [SerializeField] private GameObject m_inventorySlotPrefab;
    [SerializeField] private GameObject m_equipSlotPrefab;
    [SerializeField] private GameObject m_quickSlotPrefab;
    [Space(10)]

    [SerializeField] private Transform m_scrollerView;
    // 

    public void CreateSlot(int count)
    {

    }

    public void CreateEquipSlot()
    {

    }

}
