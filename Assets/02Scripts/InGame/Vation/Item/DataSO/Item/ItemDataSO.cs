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
        [SerializeField] private string m_id;
        [SerializeField] private string m_name;
        [SerializeField] private GameObject m_itemPrefab;
        [SerializeField] private Sprite m_icon;
        [TextArea][SerializeField] private string m_escription;
    }
}