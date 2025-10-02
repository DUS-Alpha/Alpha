using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerInCombatState : PlayerCombatState
{
    public PlayerInCombatState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Jump | InputLocoLockType.FlyUp;
    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Jump | InputLocoLockType.FlyUp;

    private int m_currentWeaponNum;
    private float m_nextDely;
    private bool m_isReloading;
    public override void Enter()
    {
        base.Enter();
        m_Combat.SetUpperAnimatorLayer(1);
        m_Combat.EnterInCombat();
        m_currentWeaponNum = m_Combat.CurrentWeaponNum;
        m_nextDely = 0;
        //m_isReloading = false;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if(m_Locomotion.IsAction || m_nextDely > 1)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            return;
        }

        if (m_Combat.IsSwapWeapon())
        {
            m_Combat.SetIsAction(true);
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_SwapWeapon);
        }
        else if (m_Combat.IsReload)
        {
            m_Combat.SetIsAction(true);
            //m_isReloading = true;
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_Reload);
        }
        else if(m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
        }
        else if (m_Combat.IsAttack)
        {
            m_nextDely = 0;
            m_Combat.Attack();
        }
        else
        {
            if (m_Combat.IsScope) m_nextDely = 0;
            else m_nextDely += Time.deltaTime;
        }

        if (m_currentWeaponNum > 1) m_Combat.SetAming(m_Combat.IsScope);
        else m_Combat.SetAming(false);
    }
    public override void Exit()
    {
        base.Exit();
        if(m_Combat.IsAction) m_Combat.SetUpperAnimatorLayer(1);
        else if(!m_Locomotion.IsFlying)
            m_Combat.SetUpperAnimatorLayer(0);

        m_Combat.ExitInCombat(m_Combat.IsAction);

        m_Combat.SetAming(false);
    }

}
