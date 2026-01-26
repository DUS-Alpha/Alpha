using alpha;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public IInventoryPort m_inventoryPort;

    public void Bind(IInventoryPort inventoryPort)
    {
        m_inventoryPort = inventoryPort;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FieldItem"))
        {
            IItemPort _itemPickup = other.GetComponent<IItemPort>();
            m_inventoryPort.AddItem(_itemPickup.Item);
        }
    }
}
