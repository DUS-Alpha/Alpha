using UnityEngine;

namespace alpha
{
    // 게임 규칙 / 흐름 판단“이 행동이 가능한가?”
    // 상태 기반 허용 / 차단
    // 데이터 변경 x, UI 호출 x
    public class InventoryCore
    {
        private InventoryMediator m_mediator;
        public InventoryCore(InventoryMediator mediator)
        {
            this.m_mediator = mediator;
        }

        public void Initialize()
        {
            
        }
    }
}