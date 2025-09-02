using UnityEngine;

public class Equipment : Item
{
    public EquipmentData EquipData => m_Data as EquipmentData;  // ItemData를 EqupmentData로 캐스팅 반환

    public virtual void Equip(GameObject user) { }      // 장착
    public virtual void Unequip(GameObject user) { }    // 해제
}
