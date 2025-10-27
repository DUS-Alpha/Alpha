using UnityEngine;


public class InventorytUIData : BaseUIData
{

}

// TODO : 인벤토리 테스트를 위한 대충만든것이므로 차후 리팩토링
public class PlayerInventoryUI : BaseUI
{
    // ========================= Equipment

    //========================= /Equipment

    private InventorytUIData m_inventoryUIData;

    // UIManager의 OpenUI에서 Call
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);


    }
}
