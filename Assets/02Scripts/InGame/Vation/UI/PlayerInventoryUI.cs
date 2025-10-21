using UnityEngine;


public class InventorytUIData : BaseUIData
{

}

// TODO : 인벤토리 테스트를 위한 대충만든것이므로 차후 리팩토링
public class PlayerInventoryUI : BaseUI
{
    // ========================= Equipment
    public EquipItemSlot HeadSlot;
    public EquipItemSlot ShieldSlot;
    public EquipItemSlot ChestArmorSlot;
    public EquipItemSlot BootsSlot;
    public EquipItemSlot GlovesSlot;
    public EquipItemSlot WeaponSlot;
    //========================= /Equipment

    private InventorytUIData m_inventoryUIData;

    // UIManager의 OpenUI에서 Call
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);


    }

    public void Equip(EquipmentDataSO equipmentData)
    {
        Sprite _icon = equipmentData.Icon;
        switch (equipmentData.EquipType)
        {
            /*case ApplicableSlots.Head:
                m_headSlot = _icon;
                break;
            case ApplicableSlots.Chest:
                m_chestSlot = _icon;
                break;
            case ApplicableSlots.Feets:
                m_feetSlot = _icon;
                break;
            case ApplicableSlots.Gloves:
                m_handSlot = _icon;
                break;*/
        }
    }
    public void UnEquip(EquipmentDataSO equipmentData)
    {
        
    }
}
