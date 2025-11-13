using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public enum EInventorySlotTypes
    {
        Inventory,
        Use,
        MeleeWeapon,
        RangeMainWeapon,
        RangeSubWeapon,
        Head,
        Gloves,
        Boots,
    }
    public class InventorySlot : MonoBehaviour
    {

        public EInventorySlotTypes SlotType;
        [Space(10)]

        [Header("Conig Settings")]
        public Image BackGround;
        public Image SlotIcon;
        public TextMeshProUGUI CountTMP;
        public int ItemCount;
        public ItemDataSO SavedItemInfoSO; // {name, prefab, 등등}

        private float m_xMin;

        public float XMin   // 좌
        {
            get
            {
                m_xMin = transform.position.x - BackGround.rectTransform.rect.width * 0.5f;
                return m_xMin;
            }
        }

        private float m_xMax;   // 우
        public float XMax
        {
            get
            {
                m_xMax = transform.position.x + BackGround.rectTransform.rect.width * 0.5f;
                return m_xMax;
            }
        }

        private float m_yMin;
        public float YMin
        {
            get
            {
                m_yMin = transform.position.y - BackGround.rectTransform.rect.height * 0.5f;
                return m_yMin;
            }
        }

        private float m_yMax;
        public float YMax
        {
            get
            {
                m_yMax = transform.position.y + BackGround.rectTransform.rect.height * 0.5f;
                return m_yMax;
            }
        }

        public bool IsInRect(Vector2 pos)
        {
            if (pos.x >= XMin && pos.x <= XMax && pos.y >= YMin && pos.y <= YMax)
                return true;

            return false;
        }
    }
}