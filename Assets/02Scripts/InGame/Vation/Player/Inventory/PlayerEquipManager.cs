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
        public Dictionary<ECountableTypes, Transform> EquipQuickHolderDic;

        // Hashtable 기반 현재 장착 아이템 관리
        public Dictionary<EWeaponTypes, WeaponItem> CurrentWeaponItems { get; private set; }
        public Dictionary<ECountableTypes, CountableItem> CurrentCountableItems { get; private set; }

        // PlayerInventoryController에서 Combat으로 스왑 가능한지 전달 및 스왑(현재 무기 데이터도 전달)
        public Func<Item> OnSwapAction;
        private int m_swapNum;
        public void InitializeModule(PlayerCore playerCore)
        {
            m_inputManager = playerCore.InputManager;
        }
        public void InitializeEvents(IPlayerEvents events)
        {
            events.CheckInputAction += CheckInput;
        }
        public void CheckInput()
        {
            m_swapNum = m_inputManager.SwapNum;
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

            EquipQuickHolderDic = new Dictionary<ECountableTypes, Transform>
            { 
                {ECountableTypes.Potion, holderDic[HolderTypes.Quick_Potion]},
                {ECountableTypes.Consumable, holderDic[HolderTypes.Quick_Consumable] },
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
                if (weapon.Key == EWeaponTypes.Melee ) weapon.Value.gameObject.SetActive(true);
                else weapon.Value.gameObject.SetActive(false);
            }
        }
        #region ======================================== Equip
        public void TryEquip(ItemDataSO data)
        {
            // 장비 장착
            switch (data)
            {
                case WeaponItemDataSO weapon:
                    EquipWeapon(weapon);
                    break;
                case ArmorItemDataSO armor:
                    EquipArmor(armor);
                    break;
                case PotionItemDataSO potion:
                    EquipQuick_Potion(potion);
                    break;
                default:
                    Debug.LogWarning("알 수 없는 아이템 타입");
                    break;
            }
            // 스탯 반영
        }

        private void EquipWeapon(WeaponItemDataSO weapon)
        {
            if (!EquipWeaponHolderDic.ContainsKey(weapon.WeaponType)) return;

            // 장착 홀더(부모)
            Transform _holder = EquipWeaponHolderDic[weapon.WeaponType];

            // 새 장착
            var _weaponItemObj = Instantiate(weapon.ItemPrefab, _holder);
            CurrentWeaponItems[weapon.WeaponType] = _weaponItemObj.GetComponent<WeaponItem>();
            
            if (CurrentWeaponItems[weapon.WeaponType] == null) 
                Debug.LogWarning($"{CurrentWeaponItems[weapon.WeaponType]} == null");
            
        }
        private void EquipArmor(ArmorItemDataSO armor)
        {

        }
        private void EquipQuick_Potion(PotionItemDataSO potion)
        {

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
        private void UnEquipWeapon(ItemDataSO data)
        {
            WeaponItemDataSO _weaponData = data as WeaponItemDataSO;
            
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

        public void QuickSlotSwap(int weaponNum)
        {
            Item _item;
            switch (weaponNum)
            {
                case 0:
                    break;
                case 1:
                    _item = CurrentWeaponItems[EWeaponTypes.Melee];
                    break;
                case 2:
                    _item = CurrentWeaponItems[EWeaponTypes.MainRange];
                    break;
                case 3:
                    _item = CurrentWeaponItems[EWeaponTypes.SubRange];
                    // 무기 교체
                    break;
                case 4:
                    _item = CurrentCountableItems[ECountableTypes.Potion];
                    break;
            }

           Item _a = OnSwapAction?.Invoke();



            /*for (int i = 0; i < m_weaponHoderTrs.Length; i++)
            {
                m_weaponHoderTrs[i].gameObject.SetActive(false);
            }
            m_weaponHoderTrs[weaponNum].gameObject.SetActive(true);*/
        }


    }
}