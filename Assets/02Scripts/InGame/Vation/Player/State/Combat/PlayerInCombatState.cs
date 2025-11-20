using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerInCombatState : PlayerCombatStateBase
{
    public PlayerInCombatState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;
    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;
    private bool m_isSniperAiming;

    private int m_currentWeapon;
    public override void Enter()
    {
        base.Enter();
        m_Combat.EnterInCombat();
        m_NextStateDelay = 0;

        m_currentWeapon = m_Combat.CurrentWeaponNum;
        // IK Rig 활성화
        if (m_currentWeapon > 1)
            m_Combat.SetIKRigWeight(RigType.Aim, true);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        base.Update();
        if (m_Locomotion.IsDie) return;
        //(!m_Combat.IsAiming && !m_Combat.IsAttack)
        if (m_Locomotion.IsCombatStop || m_NextStateDelay > 1f)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            // TODO : 모든 애니메이션 및 레이어 초기화 & 중단
            return;
        }

        if (m_Combat.IsAiming || m_Combat.IsAttack) m_NextStateDelay = 0;
        else m_NextStateDelay += Time.deltaTime;

        m_PlayerCore.AniController.SetAttackAni(m_Combat.IsAttack);

        if (m_Combat.IsSwap)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
            //m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if (m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
            //m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if(m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
            //m_PlayerCore.AniController.SetAttackAni(false);
        }
        else if(m_Combat.IsAttack)
        {
            m_Combat.Attack();
        }

        // Aim 활성화 여부
        if (m_Combat.IsAim)
            m_Combat.SetAming();

        if (m_currentWeapon == 1)
        {
            m_Combat.SetIsAction(m_PlayerCore.AniController.GetIsMeleeAttackInfo(2));
        }

        
    }
    public override void Exit()
    {
        base.Exit();
        m_PlayerCore.AniController.SetAttackAni(false);

        m_Combat.IsActioning = false;
        m_Combat.ExitInCombat(m_Locomotion.IsFlying);
        m_Combat.SetAming(true);

        m_Combat.SetIKRigWeight(RigType.Aim, false);
    }

}
