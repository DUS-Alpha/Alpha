using UnityEngine;

namespace alpha
{
    public class FieldItemModule : MonoBehaviour, IPickupItemPort
    {
        [SerializeField]
        private ItemModuleBase m_item;
        public ItemModuleBase Item => m_item;
    }
}