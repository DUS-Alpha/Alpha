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
        m_isReloading = false;
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

        //if (m_Combat.IsAction) return;

        if (m_Combat.IsSwapWeapon()) 
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_SwapWeapon);
        else if (m_Combat.IsReload)
        {
            m_isReloading = true;
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_Reload);
        }
        else if (m_Combat.IsAttack)
        {
            m_Combat.Attack();
            m_nextDely = 0;
            m_Ani.AttackAni(true, m_currentWeaponNum);
        }
        else
        {
            if (m_Combat.IsScope) m_nextDely = 0;
            else m_nextDely += Time.deltaTime;
            m_Ani.AttackAni(false, m_currentWeaponNum);
        }

        if (m_currentWeaponNum > 1) m_Combat.SetAming(m_Combat.IsScope);
        else m_Combat.SetAming(false);
    }
    public override void Exit()
    {
        base.Exit();
        if(m_isReloading) m_Combat.SetUpperAnimatorLayer(1);
        else if(!m_Locomotion.IsFlying)
            m_Combat.SetUpperAnimatorLayer(0);

        m_Combat.ExitInCombat(m_isReloading);

        m_Combat.SetAming(false);
    }

}
