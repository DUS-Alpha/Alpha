using System;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리는 아이템 보관/추가/삭제 책임만 가짐
public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance { get; private set; }

    private PlayerCore m_playerCore;
    private bool m_isInventory;

    // 선언시의 new는 Awake호출전 사용 가능
    public List<Item> SaveItemList = new List<Item>();

    public bool IsSwap { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }
    private void Start()
    {

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
        Debug.Log($"아이템 획득: {item.m_Data.Name}");
    }

    public void RemoveItem(Item item)
    {
        SaveItemList.Remove(item);
    }

    public void SaveInventory()
    {
        // Json은 string으로만 저장
        PlayerInventorySaveData saveData = new PlayerInventorySaveData(SaveItemList);
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("InventoryData", json);
    }
    public void LoadInventory()
    {
        string json = PlayerPrefs.GetString("InventoryData", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            PlayerInventorySaveData saveData = JsonUtility.FromJson<PlayerInventorySaveData>(json);
            // ScriptableObject를 다시 참조해야 하므로 ID 매핑 필요
        }
    }
}
