using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class DragSlotUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_backGO;
        [SerializeField] private Image m_itemIconImage;
        [field: SerializeField] public SlotUIBase SlotUIBase { get; private set; }

        private void Awake()
        {
            m_backGO.SetActive(false);
        }
        public void DragUI(SlotUIBase slotUIBase)
        {
            m_backGO.SetActive(true);
            SlotUIBase = slotUIBase;
            SetIcon(slotUIBase.SlotIcon.sprite);
        }

        private void SetIcon(Sprite icon)
        {
            m_itemIconImage.sprite = icon;
        }

        public void Clear()
        {
            SlotUIBase = null;
            m_itemIconImage.sprite = null;
            m_backGO.SetActive(false);
        }
    }
}