using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private Dictionary<EquipmentSlot, Equipment> equippedItems = new Dictionary<EquipmentSlot, Equipment>();

    public void EquipItem(Equipment equipment)
    {
        EquipmentSlot _slot = equipment.EquipData.Slot;

        // 해당 장비슬롯에 아이템있으면 해제
        if (equippedItems.ContainsKey(_slot))
            UnequipItem(_slot);

        // _slot키값의 equippedItems공간에 아이템(Equipment) 저장
        equippedItems[_slot] = equipment;
        equipment.Equip(gameObject);
        Debug.Log($"{equipment.m_Data.Name} 장착 완료");
    }

    public void UnequipItem(EquipmentSlot slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems[slot].Unequip(gameObject);
            Debug.Log($"{equippedItems[slot].m_Data.Name} 해제 완료");
            equippedItems.Remove(slot);
        }
    }

    public void SwapWeapon(Weapon weapon)
    {
        // 무기 슬롯 정의
        EquipmentSlot weaponSlot = EquipmentSlot.Weapon;

        // 기존 무기 해제
        if (equippedItems.ContainsKey(weaponSlot))
        {
            UnequipItem(weaponSlot);
        }

        // 새 무기 장착
        EquipItem(weapon);
    }
}
