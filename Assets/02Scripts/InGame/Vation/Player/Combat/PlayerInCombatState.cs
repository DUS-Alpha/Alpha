using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerInCombatState : PlayerCombatState
{
    public PlayerInCombatState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;
    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public override void Enter()
    {
        base.Enter();
        m_Combat.EnterInCombat();
        m_NextStateDelay = 0;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        base.Update();
        if(m_Locomotion.IsCombatStop || m_NextStateDelay > 1.5f)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            // TODO : 모든 애니메이션 및 레이어 초기화 & 중단
            return;
        }

        if (m_Combat.IsAim || m_Combat.IsAttack) m_NextStateDelay = 0;
        else m_NextStateDelay += Time.deltaTime;

        m_PlayerCore.AniController.SetAttackAni(m_Combat.IsAttack);

        if (m_Combat.IsSwap)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
            m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if (m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
            m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if(m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
            m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if(m_Combat.IsAttack)
        {
            m_Combat.Attack();
        }

        if (m_Combat.CurrentWeaponNum == 1)
        {
            m_Combat.SetIsAction(m_PlayerCore.AniController.GetIsMeleeAttackInfo(2));
        }

        m_Combat.SetAming(m_Combat.IsAim);
        
    }
    public override void Exit()
    {
        base.Exit();
        m_PlayerCore.AniController.SetAttackAni(false);

        m_Combat.IsActioning = false;
        m_Combat.ExitInCombat(m_Locomotion.IsFlying);
        m_Combat.SetAming(false, true);
    }

}
