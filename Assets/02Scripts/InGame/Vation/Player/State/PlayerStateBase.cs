using alpha;
using UnityEngine;

public abstract class PlayerStateBase
{
    protected PlayerCore m_Core;
    protected PlayerLocomotionManager m_Locomotion;
    protected PlayerCombatManager m_Combat;
    protected PlayerInputManager m_InputM;
    protected PlayerAnimationManager m_AniM;
    protected PlayerAudioManager m_Audio;

    protected float m_NextStateDelay;
    public PlayerStateBase(PlayerCore playerCore)
    {
        m_Core = playerCore;
        m_Locomotion = playerCore.LocomotionM;
        m_Combat = playerCore.Combat;
        m_InputM = playerCore.InputManager;
        m_AniM = playerCore.AniManager;
        m_Audio = playerCore.PlayerAudioManager;
    }
    public void DebugCurrentState()
    {
        Debug.Log(GetType().Name);
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
