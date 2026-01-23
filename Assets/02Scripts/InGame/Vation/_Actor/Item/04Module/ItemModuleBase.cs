using UnityEngine;

namespace alpha
{
    public class ItemModuleBase : MonoBehaviour
    {
        public ItemDataSO Data;

        // 아이템 데이터는 픽업이나 어떠한 계시로 Data를 받는 구조
        public void Initialize(ItemDataSO data) => Data = data;
    }
}