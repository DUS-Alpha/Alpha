using UnityEngine;

// PickUpItem을 가진 현재 아이템은 ItemData만 가진 상태에서
// 충돌 감지 시 ItemFactory.CreateItem에서 AddComponent로 ItemData의 클래스 추가
// 이후 인벤토리에 저장하고 픽업이후 아이템 삭제
public class PickUpItem : MonoBehaviour
{
    [SerializeField] 
    private Item m_item;  // 본인 아이템 데이터

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventoryManager _playerInventoryManager = other.GetComponent<PlayerInventoryManager>();
            _playerInventoryManager.AddItem(m_item);
            Destroy(gameObject); // 월드에서 제거
        }
    }
}
