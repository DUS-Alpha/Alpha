using UnityEngine;

public class PlayerReloadState : PlayerCombatState
{
    public PlayerReloadState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Dash;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Dash;

    private float m_nextDelay;
    private bool m_canReload;
    public override void Enter()
    {
        base.Enter();
        m_canReload = m_Combat.EnterReload();
        m_nextDelay = 0f;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if(!m_canReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            return;
        }

        m_nextDelay += Time.deltaTime;
        if (m_nextDelay < 1.5f) return;

        /*if(m_Combat.IsCombating)
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
        else*/
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
