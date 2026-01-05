using alpha;
using UnityEngine;

namespace alpha
{
    // 객체 생성 + 참조 전달 Composition Root
    // 어떤 구현체가 어떤 인터페이스를 쓰는지 결정만 함
    // 의존성 주입
    // 로직 x, 판단 x
    public class InventoryInstaller : MonoBehaviour
    {
        [SerializeField] private InventoryUIManager m_inventoryUI;
        [SerializeField] PlayerCore m_player;
        private void Awake()
        {
            var _equipModel = new EquipModel();
            var _inventoryModel = new InventoryModel(4, 4);
            var _presenter = new InventoryPresenter(m_inventoryUI);
            var _inventoryService = new InventoryService(_inventoryModel);
            var _controller = new InventoryController(_equipModel, _inventoryModel, _inventoryService);
            var _mediator = new InventoryMediator(_controller, _presenter);
            var _core = new InventoryCore(_mediator);

            //m_player.PickupController.Inject(_inventoryService);
        }
    }
}