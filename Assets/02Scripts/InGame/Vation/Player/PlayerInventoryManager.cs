using System;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리는 아이템 보관/추가/삭제 책임만 가짐
public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField]
    private Weapon[] m_testWeapons;

    // 실제 장비 슬롯 관리
    private Dictionary<ApplicableSlots, Equipment> m_slotDic = new Dictionary<ApplicableSlots, Equipment>();

    private PlayerCore m_playerCore;
    private bool m_isInventory;
    

    // 선언시의 new는 Awake호출전 사용 가능
    public List<Item> SaveItemList = new List<Item>();

    private void Awake()
    {

    }
    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    private void Start()
    {
        for (int i = 0; i< m_testWeapons.Length; i++)
        {
            AddItem(m_testWeapons[i]);
        }
    }

    public void CheckInput()
    {
        bool _isInventory = m_playerCore.InputHandler.IsInventory;
        if(_isInventory)
        {
            m_isInventory = !m_isInventory;
        }
    }

    public void AddItem(Item item)
    {
        SaveItemList.Add(item);
        // TODO : Debug 삭제예정
        //Debug.Log($"아이템 획득: {item.Data.Name}");

        // 아이템 타입 확인
        // 장비 슬롯 빈공간일 시 장착 여부
        if (item.Data.ItemType == ItemTypes.Equip)
        {
            Equipment equipment = item as Equipment;
            m_playerCore.EquipmentController.EquipItem(equipment);
        }

        // 인벤토리 UI에도 적용
    }

    public void RemoveItem(Item item)
    {
        SaveItemList.Remove(item);
    }

    // TODO : 현재 데이터관리처리는 차후에 진행
    public void SaveInventoryInfo()
    {
        // Json은 string으로만 저장
        PlayerInventorySaveData saveData = new PlayerInventorySaveData(SaveItemList);
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("InventoryData", json);
    }
    public void LoadInventoryInfo()
    {
        string json = PlayerPrefs.GetString("InventoryData", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            PlayerInventorySaveData saveData = JsonUtility.FromJson<PlayerInventorySaveData>(json);
            // ScriptableObject를 다시 참조해야 하므로 ID 매핑 필요
        }
    }
}
