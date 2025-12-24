using UnityEngine;

namespace alpha
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIInputManager m_uiInputManager;

        [SerializeField] private RealTimeUIManager m_realTimeUIManager;
        [SerializeField] private OptionMenu m_optionMenu;
        [SerializeField] private InventoryUIManager m_inventoryUIManager;

        void Start()
        {

        }

        void Update()
        {
            if(m_uiInputManager.IsInventory)
            {
                m_inventoryUIManager.gameObject.SetActive(!m_inventoryUIManager.gameObject.activeSelf);
            }
        }
    }
}