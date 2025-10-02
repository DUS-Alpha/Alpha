using UnityEngine;

public class IdleState : IEnemyState
{
    public void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("아이들 스테이트의 EnterState");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemy.TransitionToState(new ChaseState());
        }
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("exitState");
    }
}
