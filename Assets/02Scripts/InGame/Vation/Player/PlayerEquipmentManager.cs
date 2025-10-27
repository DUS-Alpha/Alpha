using System;
using System.Collections.Generic;
using UnityEngine;

// 실제 장비(모델)를 플레이어에 장착하거나 교체, 해제
public class PlayerEquipmentManager : CharacterEquipmentMnager
{
    public WeaponInstantiationSlot RightHandSlot;
    public WeaponInstantiationSlot LeftHandSlot;

    public GameObject RightHandWeapon;
    public GameObject LeftHandWeapon;

    private PlayerCore m_playerCore;
    private Transform[] m_equipmentHoderTrs;
    private Transform[] m_weaponHoderTrs;

    public WeaponItemSO[] CurrentEquipWeapons = new WeaponItemSO[3];
    //public Dictionary<WeaponTypes, WeaponSO> CurrentEquipWeaponDic;
    public void InitializeModule(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    // 코드로 받아오기
    private void InitializeWeaponSlots()
    {
        WeaponInstantiationSlot[] _weaponSlots = GetComponentsInChildren<WeaponInstantiationSlot>();
        
        foreach (var weaponSlot in _weaponSlots)
        {
            if (weaponSlot.WeaponSlotType == WeaponSlotTypes.RihgtHand)
                RightHandSlot = weaponSlot;
            else if (weaponSlot.WeaponSlotType == WeaponSlotTypes.LeftHand)
                LeftHandSlot = weaponSlot;
        }
    }
    protected override void Awake()
    {
        base.Awake();

        // Get Our Slots
        InitializeWeaponSlots();
    }
    protected override void Start()
    {
        base.Start();
        LoadWeaponOnBothHands();
    }

    public void LoadWeaponOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }
    public void LoadRightWeapon()
    {
        if(m_playerCore.InventoryManager.CurrentRightHandWeapon != null)
        {
            RightHandWeapon = Instantiate(m_playerCore.InventoryManager.CurrentRightHandWeapon.WeaponPrefab);
            RightHandSlot.LoadWeapon(RightHandWeapon);
        }
    }
    public void LoadLeftWeapon()
    {
        if (m_playerCore.InventoryManager.CurrentLeftHandWeapon != null)
        {
            LeftHandWeapon = Instantiate(m_playerCore.InventoryManager.CurrentLeftHandWeapon.WeaponPrefab);
            LeftHandSlot.LoadWeapon(LeftHandWeapon);
        }
    }

    /*public void InitializeModule()
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

       CurrentEquipWeaponDic = new Dictionary<WeaponTypes, WeaponSO>
       {
           {WeaponTypes.Melee, CurrentEquipWeapons[0] },
           {WeaponTypes.MainRange, CurrentEquipWeapons[1] },
           {WeaponTypes.SubRange, CurrentEquipWeapons[2] },
       };

       // Melee만 Holder 활성화
       foreach (var tr in m_weaponHoderTrs)
       {
           tr.gameObject.SetActive(false);
       }
   }

  // 픽업한 아이템의 데이터에 대한 아이템 정보 생성
   // 저장은 픽업아이템에서 직접 플레이어 인벤토리 클래스로 정보를 보냄
   private Item CreateItem(ItemDataSO data)
   {
       GameObject ItemObj = new GameObject(data.Name);
       Item ItemBase = null;

       if (data is WeaponDataSO weaponData)
       {
           switch (weaponData.WeaponType)
           {
               case WeaponTypes.Melee:
                   ItemBase = ItemObj.AddComponent<MeleeWeapon>();
                   break;
               case WeaponTypes.MainRange:
               case WeaponTypes.SubRange:
                   ItemBase = ItemObj.AddComponent<RangeWeapon>();
                   break;
           }
       }
       else if (data is EquipmentDataSO equipData)
           ItemBase = ItemObj.AddComponent<Equipment>();
       else
           ItemBase = ItemObj.AddComponent<Item>();

       ItemBase.Initialize(data);

       return ItemBase;
   }*/

    /// <summary>
    /// 아이템 장착 (아이템 픽업시, 인벤토리에서 장착시)
    /// </summary>
    /*public void EquipItem(Equipment equipment)
    {
        if (equipment == null || equipment.EquipData == null)
        {
            Debug.LogWarning("잘못된 Equipment 또는 EquipData입니다.");
            return;
        }

        // 실제 장비 아이템 생성
        //Item _item = CreateItem(equipment.EquipData);
        ItemSO _equipItem = GameObject.Instantiate(equipment);

        Transform _parent = GetParentTransform(equipment);

        if (_parent == null)
        {
            Debug.LogWarning($"{equipment.EquipData.Name} 의 장착 위치를 찾지 못했습니다.");
            UnityEngine.Object.Destroy(_equipItem.gameObject);
            return;
        }

        _equipItem.transform.SetParent(_parent);

        // 오브제트 초기화
        _equipItem.transform.localPosition = Vector3.zero;
        _equipItem.transform.localRotation = Quaternion.identity;
        _equipItem.transform.localScale = Vector3.one;

        if (_equipItem is WeaponSO weapon)
        {
            var _type = weapon.WeaponData.WeaponType;
            // 기존 무기가 있다면 제거
            if (CurrentEquipWeaponDic.ContainsKey(_type))
            {
                UnEquip(_type);
            }

            // 새로운 무기 등록
            CurrentEquipWeaponDic[_type] = weapon;
            int _index = (int)_type;
            if (_index < CurrentEquipWeapons.Length)
                CurrentEquipWeapons[_index] = weapon;
        }
    }
    public void UnEquip(WeaponTypes type)
    {
        if (CurrentEquipWeaponDic.TryGetValue(type, out var weapon))
        {
            if (weapon != null)
                GameObject.Destroy(weapon.gameObject);
        }

        int _index = (int)type;
        if (_index < CurrentEquipWeapons.Length)
            CurrentEquipWeapons[_index] = null;
    }

    private Transform GetParentTransform(Equipment equipment)
    {
        // 무기 장비인지 확인
        if (equipment.EquipData.EquipType == EquipTypes.Weapon && equipment is WeaponSO weapon)
            return GetWeaponHolder(weapon.WeaponData.WeaponType);

        // 일반 장비일 경우는 바로 TryGetValue로 값 적용
        return m_ammorTypeHolderDic.TryGetValue(equipment.EquipData.EquipType, out var holder)? holder : null;  //Dic의 값을 out으로 holder변수에 대입
    }

    private Transform GetWeaponHolder(WeaponTypes weaponType)
    {
        
        return m_weaponTypeHolderDic.TryGetValue(weaponType, out var holder) ? holder : null;

        *//*return weaponType switch
        {
            WeaponTypes.Melee => m_weaponHoderTrs[0],
            WeaponTypes.MainRange => m_weaponHoderTrs[1],
            WeaponTypes.SubRange => m_weaponHoderTrs[2],
            _ => null
        };*//*
    }*/

    public void SwapWeapon(int weaponNum)
    {
        weaponNum -= 1;
        
        for (int i = 0; i < m_weaponHoderTrs.Length; i++)
        {
            m_weaponHoderTrs[i].gameObject.SetActive(false);
        }
        m_weaponHoderTrs[weaponNum].gameObject.SetActive(true);
    }

}
