using UnityEngine;

public class ChaseState : IEnemyState
{
    public void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("체이스 스테이트의 EnterState");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*enemy.TransitionToState(new AttackState());*/
        }
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("EnterState");
    }
}
