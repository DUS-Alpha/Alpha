using UnityEngine;


public class Item : MonoBehaviour
{
    // 픽업이 아닌 바로 데이터 받아와 처리도 가능하게하기 위해
    public ItemData Data;

    // 픽업시에는 AddComponent로 Item을 추가하기에 Data가 null상태
    // 따라서 픽업 시Initialize가 필요
    public virtual void Initialize(ItemData data)
    {
        Data = data;
    }

    public virtual void Use(GameObject user) { }
}
