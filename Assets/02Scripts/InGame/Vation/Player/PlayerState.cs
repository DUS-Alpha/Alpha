using UnityEngine;

public abstract class PlayerState
{
    protected PlayerCore m_PlayerCore;

    public PlayerState(PlayerCore playerCore)
    {
        m_PlayerCore = playerCore;
        DebugCurrentState();
    }
    private void DebugCurrentState()
    {
        Debug.Log(GetType().Name);
    }
    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void Update();
    public abstract void Exit();
}
