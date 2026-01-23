using Unity.Collections;
using UnityEngine;

public enum EItemTypes
{
    None = 0,
    Armor = 1,
    Weapon = 2,
    Useable = 3,    // 소모품
    ingredient = 4,         // 재료
}

public enum EItemGradeTypes
{
    Normal,
    Rare,
    Unique,
    Legend
}

namespace alpha
{
    public class ItemSO : ScriptableObject
    {
        // 각 아이템데이터 클래스에서 강제 지정중
        [ReadOnly] public EItemTypes ItemType;

        [Header("[ ItemSO Info ]")]
        [SerializeField] private string m_id;
        [SerializeField] private string m_name;
        [SerializeField] private GameObject m_itemPrefab;
        [SerializeField] private Sprite m_icon;
        [SerializeField] bool m_isStatckable;
        [TextArea][SerializeField] private string m_description;

        public string ID => m_id;
        public string ItemName => m_name;
        public GameObject ItemPrefab => m_itemPrefab;
        public Sprite IconSprite => m_icon;
        public bool IsStackable => m_isStatckable;    // 아이템이 쌓을 수 있는지 여부
        public string Description => m_description;
    }
}