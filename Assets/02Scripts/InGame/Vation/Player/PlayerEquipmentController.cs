using System;
using System.Collections.Generic;
using UnityEngine;

// 실제 장비(모델)를 플레이어에 장착하거나 교체, 해제
public class PlayerEquipmentController
{
    private Transform[] m_equipmentHoderTrs;
    private Transform[] m_weaponHoderTrs;

    private Dictionary<EquipTypes, Transform> m_ammorTypeHolderDic;
    private Dictionary<WeaponTypes, Transform> m_weaponTypeHolderDic;

    public PlayerEquipmentController(Transform[] equipmentHoderTrs, Transform[] weaponHoderTrs)
    {
        m_equipmentHoderTrs = equipmentHoderTrs;
        m_weaponHoderTrs = weaponHoderTrs;
    }

    public void InitializeModule()
    {
        m_ammorTypeHolderDic = new Dictionary<EquipTypes, Transform>()
        {
            {EquipTypes.Head,  m_equipmentHoderTrs[0]},
            {EquipTypes.Chest,  m_equipmentHoderTrs[1]},
            {EquipTypes.Gloves,  m_equipmentHoderTrs[2]},
            {EquipTypes.Feets,  m_equipmentHoderTrs[3]},
        };
        m_weaponTypeHolderDic = new Dictionary<WeaponTypes, Transform>
        {
            { WeaponTypes.Melee, m_weaponHoderTrs[0] },
            { WeaponTypes.MainRange, m_weaponHoderTrs[1] },
            { WeaponTypes.SubRange, m_weaponHoderTrs[2] },
        };
    }

    /// <summary>
    /// 아이템 장착 (아이템 픽업시, 인벤토리에서 장착시)
    /// </summary>
    public void EquipItem(Equipment equipment)
    {
        if (equipment == null || equipment.EquipData == null)
        {
            Debug.LogWarning("잘못된 Equipment 또는 EquipData입니다.");
            return;
        }

        // 실제 장비 생성
        Item _item = CreateItem(equipment.EquipData);
        Transform _parent = GetParentTransform(equipment);

        if (_parent == null)
        {
            Debug.LogWarning($"{equipment.EquipData.Name} 의 장착 위치를 찾지 못했습니다.");
            UnityEngine.Object.Destroy(_item.gameObject);
            return;
        }

        _item.transform.SetParent(_parent);
    }
    private Transform GetParentTransform(Equipment equipment)
    {
        // 무기 장비인지 확인
        if (equipment.EquipData.EquipType == EquipTypes.Weapon && equipment is Weapon weapon)
            return GetWeaponHolder(weapon.WeaponData.WeaponType);

        // 일반 장비일 경우는 바로 TryGetValue로 값 적용
        return m_ammorTypeHolderDic.TryGetValue(equipment.EquipData.EquipType, out var holder)? holder : null;  //Dic의 값을 out으로 holder변수에 대입
    }

    private Transform GetWeaponHolder(WeaponTypes weaponType)
    {
        return m_weaponTypeHolderDic.TryGetValue(weaponType, out var holder) ? holder : null;

        /*return weaponType switch
        {
            WeaponTypes.Melee => m_weaponHoderTrs[0],
            WeaponTypes.MainRange => m_weaponHoderTrs[1],
            WeaponTypes.SubRange => m_weaponHoderTrs[2],
            _ => null
        };*/
    }

    public void SwapWeapon(int weaponNum)
    {

    }

    // 픽업한 아이템의 데이터에 대한 아이템 정보 생성
    // 저장은 픽업아이템에서 직접 플레이어 인벤토리 클래스로 정보를 보냄
    private Item CreateItem(ItemDataSO data)
    {
        GameObject ItemObj = new GameObject(data.Name);
        Item ItemBase;

        if (data is WeaponDataSO weaponData)
            ItemBase = ItemObj.AddComponent<Weapon>();
        else if (data is EquipmentDataSO equipData)
            ItemBase = ItemObj.AddComponent<Equipment>();
        else
            ItemBase = ItemObj.AddComponent<Item>();

        ItemBase.Initialize(data);
        return ItemBase;
    }
}
