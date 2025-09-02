using System;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    // 현재 적의 상태 (Idle, Chase, Attack 등)
    // 타입은 IEnemyState 인터페이스이지만,
    // 실제로는 IdleState, ChaseState 같은 구체 클래스 객체가 들어감
    public IEnemyState CurrentState;
    
    // 플레이어를 타겟팅하기 위한 Transform
    public Transform Target;


    void Start()
    {
        // 게임 시작 시 IdleState 상태로 전환
        // EnemyStateManager는 단순히 "IdleState다" 정도만 알고,
        // IdleState 내부에서 어떤 동작(조건 체크, 전환 로직)을 하는지는 모름
        TransitionToState(new IdleState());
        
    }

    private void Update()
    {
        // 매 프레임마다 현재 상태의 로직을 실행
        // 여기서는 "IdleState.UpdateState" 가 호출됨
        // Manager는 그 안에서 무슨 일이 일어나는지는 전혀 관여하지 않음
        CurrentState?.UpdateState(this);
    }

    public void TransitionToState(IEnemyState newState)
    {
        // 이전 상태 종료 (ExitState 호출)
        //null이면 호출을 생략  현 상태를 나간후  초기화를 하는것 
        //this는 자기 자신를 받는것  만약 현재 상태가 idleState면  idlestate.ExitState가 실행될것 
        CurrentState?.ExitState(this);

        // 새로운 상태 할당
        CurrentState = newState;

        // 새로운 상태 시작 (EnterState 호출)
        // 새로운 상태의 초기화들 타겟등  활성화 되는것들을하는것 
        // this는 자기 자신를 받는것 위에서 새로운  스테이트를 받으므로 새로 받은 state의  EnterState가 실행될것 
        CurrentState.EnterState(this);
        
        // 어떤 상태로 전환됐는지 로그
        print($"TransitionToState   + {newState}");
    }
}
