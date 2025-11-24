using System;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public enum ESlotTypes
    {
        Inventory,
        Drag,
        Armor,
        Weapon,
        Quick    // 포션 같은 사용 가능한
    }

    [Serializable]
    public struct SlotInfos
    {
        [Header("Conig Settings")]
        public Image BackGround;
        public Image SlotIcon;
        public int SlotNum;
        public int ItemCount;
        public TextMeshProUGUI CountTMP;
        public ItemDataSO ItemData;
    }

    public abstract class SlotBase : MonoBehaviour
    {
        [ReadOnly] public ESlotTypes SlotType;
        [Space(10)]

        public SlotInfos SlotInfo;
        public bool HasItem = false;

        public virtual void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO itemData)
        {
            SlotInfo.SlotNum = slotNum;
            SlotInfo.SlotIcon.sprite = icon;

            SlotInfo.ItemCount = itemCount;
            SlotInfo.CountTMP.text = itemCount.ToString();

            SlotInfo.ItemData = itemData;

            HasItem = itemData != null;

            if (HasItem && itemData.ItemType == EItemTypes.ConuntableItem)
                ActivateTMP(true);
            else
                ActivateTMP(false);

            int _childIndex = 1;
            if (SlotType == ESlotTypes.Inventory || SlotType == ESlotTypes.Drag) _childIndex = 0;

            transform.GetChild(_childIndex).gameObject.SetActive(HasItem);
        }

        public void SetSlotToItemData(int slotNum, ItemDataSO itemData)
        {
            ApplySlotInfo(slotNum, itemData.IconSprite, SlotInfo.ItemCount, itemData);
        }

        public void CopySlotInfo(int slotNum, SlotInfos slotInfos)
        {
            ApplySlotInfo(slotNum, slotInfos.SlotIcon.sprite, slotInfos.ItemCount, slotInfos.ItemData);
        }

        public void ActivateTMP(bool isTMP)
        {
            SlotInfo.CountTMP.gameObject.SetActive(isTMP);
        }
        public virtual void ClearSlot()
        {
            SlotInfo.SlotIcon.sprite = null;

            // 수량 초기화
            SlotInfo.ItemCount = 0;
            SlotInfo.CountTMP.text = "0";

            // 아이템 정보 초기화
            SlotInfo.ItemData = null;

            HasItem = false;

            // 타겟 오브젝트 비활성화
            int _childIndex = 1;
            if (SlotType == ESlotTypes.Inventory || SlotType == ESlotTypes.Drag) _childIndex = 0;
            transform.GetChild(_childIndex).gameObject.SetActive(false);
        }

        public abstract bool CanAcceptItem(ItemDataSO item);

        #region ========== ICON AREA ==========
        private float m_xMin;
        public float XMin   // 좌
        {
            get
            {
                m_xMin = transform.position.x - SlotInfo.BackGround.rectTransform.rect.width * 0.5f;
                return m_xMin;
            }
        }

        private float m_xMax;   // 우
        public float XMax
        {
            get
            {
                m_xMax = transform.position.x + SlotInfo.BackGround.rectTransform.rect.width * 0.5f;
                return m_xMax;
            }
        }

        private float m_yMin;
        public float YMin
        {
            get
            {
                m_yMin = transform.position.y - SlotInfo.BackGround.rectTransform.rect.height * 0.5f;
                return m_yMin;
            }
        }

        private float m_yMax;
        public float YMax
        {
            get
            {
                m_yMax = transform.position.y + SlotInfo.BackGround.rectTransform.rect.height * 0.5f;
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