using System;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public abstract class SlotUIBase : MonoBehaviour
    {
        [ReadOnly] public EItemTypes SlotItemType;

        [Header("Conig Settings")]
        [SerializeField] private RectTransform m_rectTr;

        public Image SlotIcon => m_slotIcon;
        [SerializeField] private Image m_slotIcon;

        public TextMeshProUGUI CountTMP => m_countTMP;
        [SerializeField] private TextMeshProUGUI m_countTMP;
        public bool IsEmpty => m_slotIcon.sprite == null;

        [field: SerializeField] public int SlotIndex { get; private set; }

        private void Awake()
        {
            m_slotIcon.gameObject.SetActive(false);
        }

        public void SetIndex(int num)
        {
            SlotIndex = num;
        }

        public void AddItem(ISlotModel slot)
        {
            if(slot.CurrentItemData == null)
            {
                Clear();
            }
            else
            {
                SetIcon(slot.CurrentItemData.IconSprite);
                SetCount(slot.CurrentItemCount);
            }
        }

        private void SetIcon(Sprite icon)
        {
            m_slotIcon.gameObject.SetActive(icon != null);
            m_slotIcon.sprite = icon;
        }

        private void SetCount(int count)
        {
            m_countTMP.text = count.ToString();
            m_countTMP.gameObject.SetActive(count > 1);
        }

        public virtual void Clear()
        {
            SetIcon(null);
            SetCount(0);
        }

        #region ========== ICON DRAG AREA ==========
        private float m_xMin;
        public float XMin   // 좌
        {
            get
            {
                m_xMin = transform.position.x - m_rectTr.rect.width * 0.5f;
                return m_xMin;
            }
        }

        private float m_xMax;   // 우
        public float XMax
        {
            get
            {
                m_xMax = transform.position.x + m_rectTr.rect.width * 0.5f;
                return m_xMax;
            }
        }

        private float m_yMin;
        public float YMin
        {
            get
            {
                m_yMin = transform.position.y - m_rectTr.rect.height * 0.5f;
                return m_yMin;
            }
        }

        private float m_yMax;
        public float YMax
        {
            get
            {
                m_yMax = transform.position.y + m_rectTr.rect.height * 0.5f;
                return m_yMax;
            }
        }

        public bool IsInRect(Vector2 pos)
        {
            if (pos.x >= XMin && pos.x <= XMax && pos.y >= YMin && pos.y <= YMax)
                return true;

            return false;
        }
        #endregion ========== /Icon Area ==========
    }
}