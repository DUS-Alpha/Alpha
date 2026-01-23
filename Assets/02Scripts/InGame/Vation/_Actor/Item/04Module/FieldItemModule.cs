using UnityEngine;

namespace alpha
{
    public class FieldItemModule : MonoBehaviour, IItemPort
    {
        [SerializeField]
        private ItemModuleBase m_item;
        public ItemModuleBase Item => m_item;
    }
}