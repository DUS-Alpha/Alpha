using UnityEngine;

public class PlayerCombatIdleState : PlayerState
{
    public PlayerCombatIdleState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Combat.SwapWeapon();
    }

    public override void Exit()
    {
        
    }
}
