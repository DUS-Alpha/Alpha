using UnityEngine;
using UnityEngine.Windows;
public class PlayerSkillState : PlayerCombatState
{
    public PlayerSkillState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter
    {
        get
        {
            if (m_Combat.CurrentWeaponNum == 1) return InputLocoLockType.All;
            else return InputLocoLockType.None;
        }
    }

    protected override InputLocoLockType m_LockOnExit
    {
        get
        {
            if (m_Combat.CurrentWeaponNum == 1) return InputLocoLockType.All;
            else return InputLocoLockType.None;
        }
    }
    private bool m_isMelee;
    private float m_nextDelay;

    public override void Enter()
    {
        base.Enter();

        if (m_Combat.CurrentWeaponNum == 1) m_isMelee = true;

        m_PlayerCore.AniController.SetAnimatorWeight(3,1);

        m_Combat.EnterSkill();
        m_nextDelay = 0;
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.IsAction) return;

        /*m_nextDelay += Time.deltaTime;

        if (m_nextDelay < 0.2f) return;*/

        if (m_Combat.IsAttack) m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
        else
        m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
    }
    

    public override void Exit()
    {
        base.Exit();
        if (m_isMelee) m_PlayerCore.AniController.SetAnimatorWeight(3, 0);
        m_Combat.SkillQueue.Clear();
    }
}
