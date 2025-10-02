using UnityEngine;

public class Equipment : Item
{
    // 두 캐스팅으로한 이유는 Initialize로 했을 시 받아오는 Data는 클래스 생성시 Awake로 불러와도 더 빠른 Call 구조
    // ItemData를 먼저 생성하고 Initialize로 데이터 만든 후 Equipment 클래스를 추가해서 넣어줘야하는 구조임
    public EquipmentDataSO EquipData => (Data as EquipmentDataSO);

    public virtual void Equip(GameObject user) { }      // 장착
    public virtual void Unequip(GameObject user) { }    // 해제
}
