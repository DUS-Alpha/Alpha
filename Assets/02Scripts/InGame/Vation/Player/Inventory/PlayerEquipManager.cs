using System;
using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    [System.Serializable]
    public class WeaponHolder
    {
        public EWeaponTypes WeaponType;
        public Transform HolderTransform;
    }

    [System.Serializable]
    public class QuickHolder
    {
        public ECountableTypes QuickType;
        public Transform HolderTransform;
    }

    // 실제 장비(모델)를 플레이어에 장착하거나 교체, 해제
    public class PlayerEquipManager : MonoBehaviour
    {
        private PlayerInputManager m_inputManager;

        // ========== Equip Holder ==========
        [SerializeField] private ItemHolder[] m_itemholder;
        private Dictionary<HolderTypes, Transform> holderDic = new();       // 각 타입 홀더들 저장

        // ========== Equip Item Holder ========== 
        public Dictionary<EWeaponTypes, Transform> EquipWeaponHolderDic;    // 아이템 생성될 홀더(부모)객체 저장(holder Type과 EWeaponTypes/ECountableTypes 각 매칭)
        public Dictionary<ECountableTypes, Transform> EquipCountableHolderDic;

        // 현재 장착 아이템 관리
        public Dictionary<EWeaponTypes, WeaponItem> CurrentWeaponItems { get; private set; }
        public Dictionary<ECountableTypes, CountableItem> CurrentCountableItems { get; private set; }

        // PlayerInventoryController에서 Combat으로 스왑 가능한지 전달 및 스왑(현재 무기 데이터도 전달)
        private Item m_currentIntem;
        public void InitializeModule(PlayerCore playerCore)
        {
            m_inputManager = playerCore.InputManager;
        }

        private void Awake()
        {
            m_itemholder = GetComponentsInChildren<ItemHolder>();

            foreach (var itemHolder in m_itemholder)
            {
                if (itemHolder.HolderType != HolderTypes.None)
                {
                    holderDic[itemHolder.HolderType] = itemHolder.transform;
                }
            }

            // 장착할 홀더(슬롯) 구분
            EquipWeaponHolderDic = new Dictionary<EWeaponTypes, Transform>
            { 
                {EWeaponTypes.Melee, holderDic[HolderTypes.Melee] },
                {EWeaponTypes.MainRange, holderDic[HolderTypes.MainRange]},
                {EWeaponTypes.SubRange, holderDic[HolderTypes.SubRange]}
            };

            EquipCountableHolderDic = new Dictionary<ECountableTypes, Transform>
            { 
                {ECountableTypes.Potion, holderDic[HolderTypes.Countable_Potion]},
                {ECountableTypes.Consumable, holderDic[HolderTypes.Countable_Consumable] },
            };

            // 현재 장착 아이템 테이블
            CurrentWeaponItems = new Dictionary<EWeaponTypes, WeaponItem>();
            foreach (var type in Enum.GetValues(typeof(EWeaponTypes)))
            {
                CurrentWeaponItems[(EWeaponTypes)type] = null;
            }

            CurrentCountableItems = new Dictionary<ECountableTypes, CountableItem>();
            foreach (var type in Enum.GetValues(typeof(ECountableTypes)))
            {
                CurrentCountableItems[(ECountableTypes)type] = null;
            }
        }

        private void Start()
        {
            foreach(var weapon in EquipWeaponHolderDic)
            {
                weapon.Value.gameObject.SetActive(false);
            }

            foreach (var countable in EquipCountableHolderDic)
            {
                countable.Value.gameObject.SetActive(false);
            }
        }
        #region ======================================== Equip
        public void TryEquip(ItemDataSO data)
        {
            // 장비 장착
            switch (data)
            {
                case WeaponItemDataSO weaponData:
                    EquipWeapon(weaponData);
                    break;
                case ArmorItemDataSO armorData:
                    EquipArmor(armorData);
                    break;
                case PotionItemDataSO potionData:
                    EquipQuick_Potion(potionData);
                    break;
                default:
                    Debug.LogWarning("알 수 없는 아이템 타입");
                    break;
            }
            // 스탯 반영
        }

        private void EquipWeapon(WeaponItemDataSO weaponData)
        {
            if (!EquipWeaponHolderDic.ContainsKey(weaponData.WeaponType)) return;

            // 장착 홀더(부모)
            Transform _holder = EquipWeaponHolderDic[weaponData.WeaponType];

            // 새 장착
            var _weaponItemObj = Instantiate(weaponData.ItemPrefab, _holder);
            WeaponItem _weaponItem = _weaponItemObj.GetComponent<WeaponItem>();
            _weaponItem.Initialize(weaponData);

            CurrentWeaponItems[weaponData.WeaponType] = _weaponItem;
            
            if (CurrentWeaponItems[weaponData.WeaponType] == null) 
                Debug.LogWarning($"{CurrentWeaponItems[weaponData.WeaponType]} == null");
            
        }
        private void EquipArmor(ArmorItemDataSO armorData)
        {

        }
        private void EquipQuick_Potion(PotionItemDataSO potionData)
        {
            if (!EquipCountableHolderDic.ContainsKey(potionData.CountableType)) return;

            Transform _holder = EquipCountableHolderDic[potionData.CountableType];

            var _countablePotionObj = Instantiate(potionData.ItemPrefab, _holder);
            CountableItem _countableItem = _countablePotionObj.GetComponent<CountableItem>();

            _countableItem.Initialize(potionData);

            CurrentCountableItems[potionData.CountableType] = _countableItem;

            if(CurrentCountableItems[potionData.CountableType] == null)
                Debug.LogWarning($"{CurrentCountableItems[potionData.CountableType]} == null");
        }
        #endregion ======================================== / Equip

        #region ======================================== UnEquip
        public void TryUnEquip(ItemDataSO data)
        {
            switch (data)
            {
                case WeaponItemDataSO weapon:
                    UnEquipWeapon(weapon);
                    break;
                case ArmorItemDataSO armor:
                    //UnEquipArmor(armor);
                    break;
                case PotionItemDataSO potion:
                    UnEquipQuick_Potion(potion);
                    break;
                default:
                    Debug.LogWarning("알 수 없는 아이템 타입");
                    break;
            }
            
        }
        private void UnEquipWeapon(WeaponItemDataSO data)
        {
            WeaponItemDataSO _weaponData = data;
            
            if (CurrentWeaponItems[_weaponData.WeaponType] != null)
            {
                Destroy(CurrentWeaponItems[_weaponData.WeaponType].gameObject);
                CurrentWeaponItems[_weaponData.WeaponType] = null;
            }
        }
        private void UnEquipQuick_Potion(ItemDataSO data)
        {

        }

        #endregion ======================================== / UnEquip
        private Item GetItemBySwapNum(int swapNum)
        {
            return swapNum switch
            {
                1 => CurrentWeaponItems[EWeaponTypes.Melee],
                2 => CurrentWeaponItems[EWeaponTypes.MainRange],
                3 => CurrentWeaponItems[EWeaponTypes.SubRange],
                4 => CurrentCountableItems[ECountableTypes.Potion],
                _ => null
            };
        }

        private Transform GetHolderBySwapNum(int swapNum)
        {
            return swapNum switch
            {
                1 => EquipWeaponHolderDic[EWeaponTypes.Melee],
                2 => EquipWeaponHolderDic[EWeaponTypes.MainRange],
                3 => EquipWeaponHolderDic[EWeaponTypes.SubRange],
                4 => EquipCountableHolderDic[ECountableTypes.Potion],
                _ => null
            };
        }

        public bool CanSwap(int swapNum)
        {
            m_currentIntem = GetItemBySwapNum(swapNum);
            if (m_currentIntem == null) return false;

            return true;
        }

        public Item TrySwap(int swapNum)
        {
            int _swapNum = swapNum;

            // 1. 모든 WeaponHolder 비활성화
            foreach (var kvp in EquipWeaponHolderDic)
                kvp.Value.gameObject.SetActive(false);

            // 2. 모든 QuickSlotHolder 비활성화
            foreach (var kvp in EquipCountableHolderDic)
                kvp.Value.gameObject.SetActive(false);

            // 3. swapNum으로 선택된 홀더 가져오기
            Transform targetHolder = GetHolderBySwapNum(_swapNum);

            if (targetHolder == null)
            {
                Debug.LogWarning($"TrySwap: [{_swapNum}]에 해당하는 홀더가 존재하지 않습니다.");
                return null;
            }

            // 4. 해당 홀더만 활성화
            targetHolder.gameObject.SetActive(true);

            return m_currentIntem;
        }


    }
}