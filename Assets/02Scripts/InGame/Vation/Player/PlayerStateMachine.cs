using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    //private PlayerCore m_playerCore;

    public void Initialize(PlayerCore playerCore)
    {
        //m_playerCore = playerCore;
    }

    private void Update()
    {
        CurrentState.Update();
    }

    public void SwitchState(PlayerState newState)
    {
        if (newState == CurrentState) return;
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
