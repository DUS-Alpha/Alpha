using UnityEngine;
using static UnityEngine.InputManagerEntry;

namespace alpha
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerCore m_playerCore;
        [SerializeField] private InventoryView m_inventoryView;
        
        private void Awake()
        {
            m_playerCore.Bind(m_inventoryView);
            m_inventoryView.Bind(m_playerCore.InventoryM);
        }
    }
}