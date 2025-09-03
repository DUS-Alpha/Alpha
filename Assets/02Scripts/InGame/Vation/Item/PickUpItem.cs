using UnityEngine;

// PickUpItem을 가진 현재 아이템은 ItemData만 가진 상태에서
// 충돌 감지 시 ItemFactory.CreateItem에서 AddComponent로 ItemData의 클래스 추가
// 이후 인벤토리에 저장하고 픽업이후 아이템 삭제
public class PickUpItem : MonoBehaviour
{
    [SerializeField] 
    private ItemData m_itemData;  // 본인 아이템 데이터

    // 픽업한 아이템의 데이터에 대한 아이템 정보 생성
    // 저장은 픽업아이템에서 직접 플레이어 인벤토리 클래스로 정보를 보냄
    private Item CreateItem(ItemData data)
    {
        GameObject ItemObj = new GameObject(data.Name);
        Item ItemBase;

        if (data is WeaponData weaponData)
            ItemBase = ItemObj.AddComponent<Weapon>();
        else if (data is EquipmentData equipData)
            ItemBase = ItemObj.AddComponent<Equipment>();
        else
            ItemBase = ItemObj.AddComponent<Item>();

        ItemBase.Initialize(data);
        return ItemBase;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            Item _itemInstance = CreateItem(m_itemData);
            PlayerInventoryManager _playerInventoryManager = other.GetComponent<PlayerInventoryManager>();
            Debug.Log($"other: {other.name}, parent: {other.transform.root.name}");
            _playerInventoryManager.AddItem(_itemInstance);
            Destroy(gameObject); // 월드에서 제거
        }
    }
}
