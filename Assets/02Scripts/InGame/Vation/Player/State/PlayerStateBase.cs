using alpha;
using UnityEngine;

public abstract class PlayerStateBase
{
    protected PlayerCore m_PlayerCore;
    protected PlayerLocomotion m_Locomotion;
    protected PlayerCombat m_Combat;
    protected PlayerInputManager m_InputM;
    protected PlayerStateMachine m_StateMachine;

    protected float m_NextStateDelay;
    public PlayerStateBase(PlayerCore playerCore)
    {
        m_PlayerCore = playerCore;
        m_Locomotion = playerCore.Locomotion;
        m_Combat = playerCore.Combat;
        m_InputM = playerCore.InputManager;
        m_StateMachine = playerCore.StateMachine;
        //DebugCurrentState();
    }
    public void DebugCurrentState()
    {
        Debug.Log(GetType().Name);
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
