using System.Collections.Generic;
using UnityEngine;

// 실제 장비(모델)를 플레이어에 장착하거나 교체, 해제
public class PlayerEquipmentController : MonoBehaviour
{
    private Dictionary<ApplicableSlots, Equipment> m_currentEquippedItemDic = new Dictionary<ApplicableSlots, Equipment>();
    private Dictionary<ApplicableSlots, Transform> m_holderTrDic;
    [SerializeField]
    private Transform m_headHolderTr;
    [SerializeField]
    private Transform m_chestHolderTr;
    [SerializeField]
    private Transform m_handHolderTr;
    [SerializeField]
    private Transform m_feetHolderTr;
    //InputKey값과 동일하게하기 위해 HandHolder추가
    // TODO : 메모리 공간 최적화
    [Tooltip("모델 RightAttach 0:Hand, 1:Melee, 2:Rifle, 3:Sniper"),SerializeField]  
    private Transform[] m_weaponHolderTr = new Transform[4];

    public void InitializeModule()
    {
        
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.SwapWeaponAction += SwapWeapon;
    }

    private void Awake()
    {
        m_holderTrDic = new Dictionary<ApplicableSlots, Transform>
        {
            { ApplicableSlots.Head, m_headHolderTr},
            { ApplicableSlots.Chest, m_chestHolderTr},
            { ApplicableSlots.Hands, m_chestHolderTr},
            { ApplicableSlots.Feets, m_headHolderTr},
            { ApplicableSlots.MeleeWeapon, m_weaponHolderTr[1]},
            { ApplicableSlots.RifleWeapon, m_weaponHolderTr[2]},
            { ApplicableSlots.SniperWeapon, m_weaponHolderTr[3]},
        };
    }

    private void Start()
    {
        SwapWeapon(0);
    }

    public void EquipItem(Equipment equipment)
    {
        ApplicableSlots _slot = equipment.EquipData.ApplicableSlot;

        // 해당 장비슬롯에 아이템있으면 슬롯에서 해제
        if (m_currentEquippedItemDic.ContainsKey(_slot)){UnequipItem(_slot);}

        // _slot키값의 equippedItems공간에 아이템(Equipment) 저장
        m_currentEquippedItemDic[_slot] = equipment;

        // 실제 무기 생성
        CreateEquipment(equipment);

        equipment.Equip(gameObject);
        //Debug.Log($"{equipment.EquipData.Name} 장착 완료");
    }

    public void UnequipItem(ApplicableSlots slot)
    {
        if (m_currentEquippedItemDic.ContainsKey(slot))
        {
            m_currentEquippedItemDic[slot].Unequip(gameObject);
            Debug.Log($"{m_currentEquippedItemDic[slot].EquipData.Name} 해제 완료");

            // 첫번째 자식 삭제 (현재 장착되어 있는)
            RemoveEquipment(slot);

            m_currentEquippedItemDic.Remove(slot);
        }
    }

    public void SwapWeapon(int weaponSlotIndex)
    {
        for (int i = 0; i < m_weaponHolderTr.Length; i++)
        {
            if(weaponSlotIndex == i)
                m_weaponHolderTr[i].gameObject.SetActive(true);
            else
                m_weaponHolderTr[i].gameObject.SetActive(false);
        }

        // TODO : 리팩토링때 자세히 처리
        /*// 무기 슬롯 정의
        ApplicableSlots weaponSlot = ApplicableSlots.Weapon;

        // 기존 무기 해제
        if (equippedItems.ContainsKey(weaponSlot))
        {
            UnequipItem(weaponSlot);
        }

        // 새 무기 장착
        EquipItem(weapon);*/
    }
    
    public void CreateEquipment(Equipment equipment)
    {
        Transform _parents = m_holderTrDic[equipment.EquipData.ApplicableSlot];
         Instantiate(equipment.Data.ItemPrefab, _parents);
    }
    public void RemoveEquipment(ApplicableSlots slot)
    {
        Transform _parentHolder = m_holderTrDic[slot];
        Destroy(_parentHolder.GetChild(0).gameObject);
    }
}
