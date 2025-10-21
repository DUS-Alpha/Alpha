using System.Collections.Generic;
using UnityEngine;

// TODO : 현재는 전투중심의 시스템을 먼저 개발
public class PlayerInventoryManager : MonoBehaviour
{
    private PlayerCore m_playerCore;
    public PlayerEquipmentController EquipmentController { get; private set; }

    [SerializeField]
    private Weapon[] m_testWeapons;

    // 인벤토리에 저장된 아이템들
    public List<Item> SaveInventoryItemList = new List<Item>();

    [Header("[ Holder ]")]
    // 실제 장비 착용위치
    [SerializeField]
    private Transform[] m_equipmentHoderTrs;
    [SerializeField]
    private Transform[] m_weaponHoderTrs;

    [Tooltip(" 하이어라키검색용 "),SerializeField]
    private ItemHolder[] m_itemHolders;
    public bool IsInventory { get; private set;}

    public Weapon[] Weapons;
    public void InitializeModule(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
        events.SwapWeaponAction += SwapWeapon;
    }

    private void Awake()
    {
        Weapons = new Weapon[3];
        // HolderTr들 EquipmentController로 전달
        EquipmentController = new PlayerEquipmentController(m_equipmentHoderTrs, m_weaponHoderTrs);
        EquipmentController.InitializeModule();
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
            IsInventory = !IsInventory;
        }
    }

    public void AddItem(Item item)
    {
        SaveInventoryItemList.Add(item);

        // 아이템 타입 확인
        // 장비 슬롯 빈공간일 시 장착
        if(item.Data.ItemType == ItemTypes.Equipment)
        {
            Equipment equipment = item as Equipment;
            EquipmentController.EquipItem(equipment);
        }
    }

    public void SwapWeapon(int weaponNum)
    {
        EquipmentController.SwapWeapon(weaponNum);
        Weapons = EquipmentController.CurrentEquipWeapons;
    }


    public void RemoveItem(Item item)
    {
        SaveInventoryItemList.Remove(item);
    }

    // TODO : 현재 데이터관리처리는 차후에 진행
    public void SaveInventoryInfo()
    {
        // Json은 string으로만 저장
        PlayerInventorySaveData saveData = new PlayerInventorySaveData(SaveInventoryItemList);
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

    public void OpenInventory()
    {
        
    }

    private void OnValidate()
    {
        m_itemHolders = GetComponentsInChildren<ItemHolder>();
        // TODO : 좀 더 체계적으로 변환할것
        foreach (var itemHolder in m_itemHolders)
        {
            switch (itemHolder.HolderType)
            {
                case HolderTypes.None:
                    break;
                case HolderTypes.Head:
                    m_equipmentHoderTrs[0] = itemHolder.transform;
                    break;
                case HolderTypes.Chest:
                    m_equipmentHoderTrs[1] = itemHolder.transform;
                    break;
                case HolderTypes.Gloves:
                    m_equipmentHoderTrs[2] = itemHolder.transform;
                    break;
                case HolderTypes.Feets:
                    m_equipmentHoderTrs[3] = itemHolder.transform;
                    break;
                case HolderTypes.Melee:
                    m_weaponHoderTrs[0] = itemHolder.transform;
                    break;
                case HolderTypes.MainRange:
                    m_weaponHoderTrs[1] = itemHolder.transform;
                    break;
                case HolderTypes.SubRange:
                    m_weaponHoderTrs[2] = itemHolder.transform;
                    break;
            }
        }
    }
}
