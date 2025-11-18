using Unity.Collections;
using UnityEngine;

public enum EItemTypes
{
    ConuntableItem,    // 소모품
    EquipmentItem
}

public enum EItemGradeTypes
{
    Normal,
    Rare,
    Unique,
    Legend
}

/*
[상속 구조]
ItemData
- CountableItemData
    ㄴPortionItemData
- EquipmentItemData
    ㄴWeaponItemData
        ㄴMeleeItemData
        ㄴRangeItemData
 */

namespace alpha
{
    public abstract class ItemDataSO : ScriptableObject
    {
        [ReadOnly] public EItemTypes ItemType;
        public string ID => m_id;
        public string Name => m_name;
        public GameObject ItemPrefab => m_itemPrefab;
        public Sprite IconSprite => m_icon;
        public string Description => m_escription;

        [Header("[ ItemDataSO Info ]")]
        public string m_id;
        public string m_name;
        public GameObject m_itemPrefab;
        public Sprite m_icon;
        [TextArea] public string m_escription;

        /// <summary> 타입에 맞는 새로운 아이템 생성 </summary>
        public abstract Item CreateItem();
    }
}