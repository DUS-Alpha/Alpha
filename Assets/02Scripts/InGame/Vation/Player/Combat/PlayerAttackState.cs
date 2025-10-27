using UnityEngine;

public class PlayerAttackState : PlayerCombatState
{
    public PlayerAttackState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => throw new System.NotImplementedException();

    protected override InputLocoLockType m_LockOnExit => throw new System.NotImplementedException();

    WeaponItemSO m_currentWeapon;
    public override void Enter()
    {
        base.Enter();
       
    }
    public override void FixedUpdate()
    {
        
    }
    public override void Update()
    {
        base.Update();
        if (m_Locomotion.IsDie) return;
        
        

        if (m_Combat.IsSwap)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        else if (m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
        }
        else if (m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
        }
        else if(m_Combat.IsAttack)
            m_Combat.Attack();
    }
    public override void Exit()
    {
        base.Exit();

    }
}
