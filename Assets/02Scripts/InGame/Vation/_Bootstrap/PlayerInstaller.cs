using UnityEngine;

namespace alpha
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerCore m_playerCore;
        [SerializeField] private InventoryView m_inventoryView;
        
        private void Start()
        {
            m_playerCore.Bind(m_inventoryView);
            m_inventoryView.Bind(m_playerCore.InventoryM);
        }
    }
}