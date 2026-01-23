using UnityEngine;
using Unity.Collections;
/*
상속
CountableItemDataSO
    ㄴPostionDataSO
*/
namespace alpha
{
    public enum ECountableTypes
    {
        Potion,

        /// <summary>
        /// 일반 소모품, 음식, 버프 아이템 등
        /// </summary>
        Consumable,      
        
        Ammo,

        /// <summary>
        /// 제작 재료
        /// </summary>
        CraftingMaterial
    }
    public abstract  class UseableSO : ItemSO
    {
        public ECountableTypes CountableType;
        public int MaxAmount => m_maxAmount;
        [SerializeField] private int m_maxAmount = 99;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            ItemType = EItemTypes.Useable;
        }
#endif
    }
}