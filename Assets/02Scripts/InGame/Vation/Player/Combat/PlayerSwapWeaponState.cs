using UnityEngine;

public class PlayerSwapWeaponState : PlayerState
{
    public PlayerSwapWeaponState(PlayerCore playerCore) : base(playerCore){}

    private float delayTime;

    public override void Enter()
    {
        delayTime = 0;
        m_Combat.SwapWeapon();
    }


    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        delayTime += Time.deltaTime;
        if (delayTime > 0.25f)
        {
            m_PlayerCore.SwitchCombatState(CombatState.CombatIdle);
        }
    }
    public override void Exit()
    {
        
    }
}
