using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class DragSlot : MonoBehaviour
    {
        [Header("[ Settings ]")]
        public Image SavedIcon;
        public TextMeshProUGUI SavedCountTMP;
        [Space(10)]

        [Header("[ Leave blank ]")]
        public int SavedItemCount;
        public int SavedSlotNum;
        public ItemDataSO SavedItemInfo;

        public void InitializeDragSlot()
        {
            SavedIcon.sprite = null;
            SavedItemCount = 0;
            SavedCountTMP.text = "0";
            SavedItemInfo = null;
            gameObject.SetActive(false);
        }
    }
}