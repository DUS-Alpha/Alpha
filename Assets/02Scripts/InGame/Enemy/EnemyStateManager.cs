using System;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public IEnemyState CurrentState;


    void Start()
    {
        TransitionToState(new IdleState());
    }

    private void Update()
    {
        CurrentState?.UpdateState(this);
    }

    public void TransitionToState(IEnemyState newState)
    {
        CurrentState?.ExitState(this);
        CurrentState = newState;
        CurrentState.EnterState(this);
        print($"TransitionToState   + {newState}");
    }
}
