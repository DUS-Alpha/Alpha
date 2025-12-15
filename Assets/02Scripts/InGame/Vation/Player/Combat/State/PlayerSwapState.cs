using alpha;
using UnityEngine;

public class PlayerSwapState : PlayerCombatStateBase
{
    public PlayerSwapState(PlayerCore playerCore) : base(playerCore){}
 
    public override void Enter()
    {
        // 스왑 아이템 Combat에 전달 (무기 아이템 CombatManager에서 전략패턴 사용을 위해)
        int _swapNum = m_InputM.SwapNum;
        Item _item = m_Core.EquipmentManager.TrySwapAndGetItem(_swapNum);
        m_Combat.SetCurrentSwapItem(_item);

        // 애니메이션
        m_AniM.SwapWeaponAni(_swapNum, false);
    }

    public override void Update()
    {
        m_Combat.InvokeRegenerateGauge();
        if (m_Combat.IsAction) return;

        if(m_Combat.IsInCombat) m_Core.SwitchCombatState(CombatStateType.CombatReady);
        else m_Core.SwitchCombatState(CombatStateType.NonCombat);
    }

    public override void Exit()
    {
        //m_Combat.ExitSwap(m_Locomotion.IsFlying);
    }
}
