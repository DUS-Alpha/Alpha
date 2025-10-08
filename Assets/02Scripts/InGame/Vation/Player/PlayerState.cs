using UnityEngine;

public abstract class PlayerState
{
    protected PlayerCore m_PlayerCore;
    protected PlayerLocomotion m_Locomotion;
    protected PlayerCombat m_Combat;
    protected PlayerAnimationController m_Ani;
    protected float m_nextStateDelay;
    public PlayerState(PlayerCore playerCore)
    {
        m_PlayerCore = playerCore;
        m_Locomotion = playerCore.Locomotion;
        m_Combat = playerCore.Combat;
        m_Ani = playerCore.AniController;
        //DebugCurrentState();
    }
    public void DebugCurrentState()
    {
        Debug.Log(GetType().Name);
    }

    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
}
