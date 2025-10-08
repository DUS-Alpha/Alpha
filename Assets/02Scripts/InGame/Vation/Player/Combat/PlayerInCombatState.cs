using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerInCombatState : PlayerCombatState
{
    public PlayerInCombatState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Jump | InputLocoLockType.FlyUp;
    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Jump | InputLocoLockType.FlyUp;

    public override void Enter()
    {
        base.Enter();
        m_Combat.EnterInCombat();
        m_nextStateDelay = 0;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        base.Update();
        if(m_Locomotion.IsCombatStop || m_nextStateDelay > 1)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            // TODO : 모든 애니메이션 및 레이어 초기화 & 중단
            return;
        }

        if (m_Combat.CurrentWeaponNum > 1)
        {
            if(m_Combat.IsAim) m_nextStateDelay = 0;
            m_Combat.SetAming(m_Combat.IsAim);
        }

        if (m_Combat.IsSwap)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        else if (m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
        }
        else if(m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
        }
        else if (m_Combat.IsAttack)
        {
            m_nextStateDelay = 0;
            m_Combat.Attack();
            m_Combat.UpdateInCombat(m_Locomotion.IsFlying);
        }
        else
        {
            m_nextStateDelay += Time.deltaTime;
        }

        m_Combat.SetAttack(m_Combat.IsAttack);
    }
    public override void Exit()
    {
        base.Exit();
        
        if(!m_Locomotion.IsFlying)
            m_Combat.SetUpperAnimatorLayer(0);

        m_Combat.ExitInCombat();
        m_Combat.SetAming(false, true);
    }

}
