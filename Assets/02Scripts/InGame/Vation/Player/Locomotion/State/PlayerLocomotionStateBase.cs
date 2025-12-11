using UnityEngine;

public abstract class PlayerLocomotionStateBase : PlayerStateBase
{
    public PlayerLocomotionStateBase(PlayerCore playerCore) : base(playerCore){}

    public abstract override void Enter();
    public abstract override void Update();
    public abstract override void Exit();
}
