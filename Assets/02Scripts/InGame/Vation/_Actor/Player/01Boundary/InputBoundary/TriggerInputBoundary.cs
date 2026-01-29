using alpha;
using UnityEngine;

public class TriggerInputBoundary : MonoBehaviour
{
    public IInventoryPickupPort m_inventoryPort;

    public void Bind(IInventoryPickupPort inventoryPort)
    {
        m_inventoryPort = inventoryPort;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FieldItem"))
        {
            if (!other.TryGetComponent<IPickupItemPort>(out var pickupItemPort)) return;

            m_inventoryPort.AddItem(pickupItemPort.Item);
            Destroy(other.gameObject);
        }
    }
}
