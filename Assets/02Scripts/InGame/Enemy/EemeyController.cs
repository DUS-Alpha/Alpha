using System.Collections.Generic;
using UnityEngine;

public class EemeyController : MonoBehaviour
{
    private BehaviorTreeRunner m_runner;
    private BossActions m_actions;
    [SerializeField] Transform m_player; // 플레이어
    
    void Start()
    {
        m_runner = GetComponent<BehaviorTreeRunner>();
        m_actions = GetComponent<BossActions>();
        // ★ 추가: 블랙보드 생성 및 기본 연결
        var _bb = new Blackboard(gameObject)
        {
            // 필요하면 여기서 초기화 (예: Target을 EnemyStateManager에서 끌어와 연결)
            // EnemyStateManager에 target이 있다면:
            // Target = enemy.target
            Target = m_player.transform, // 플레이어 
            Owner =  gameObject, //자기 자신 -> 보스 애너미
            OwnerTransform = gameObject.transform,
        };
        m_actions.SetBlackboard(_bb);
        
               var subNode = new List<INode>
               {
                   new ActionNode(m_actions.CloseRandomAni),
                   new ActionNode(m_actions.MiddleRandomAni),
                   new ActionNode(m_actions.FarRandomAni)
               };
               
               INode Move =
                   new SequenceNode(
                   new ActionNode(m_actions.Move),
                   new ActionNode(m_actions.SearchRange),
                   new SelectorNode(subNode.ToArray())
                   
               );
               
        
               
               /*
                * 1.패턴을 실행할 조건이 되면 보스가 패턴을 실행할  위치로 먼저 이동
                * 2.그 후 격돌 위치를 보여주기
                * 3.GateOpen인 Bool 변수가 참이면 성공 거짓이면 계속 하도록 Running  -> 차후 일정 시간동안 거짓 일시 실패하는 기능을 구현
                * 4.격돌 위치 안보이게 만듬
                * 5.격돌 실행
                */
            
               
               INode specialFlow2 = new SequenceNode(
                   new ActionNode(m_actions.ConditionPattern),     // 패턴 포지션으로 이동/정렬
                   new ActionNode(m_actions.StartGateWindow),      // 2초 대기 윈도우 시작(장판 ON)
                   new SelectorNode(
                       // [성공 브랜치] 2초 안에 G 눌러 GateOpen == true
                       new SequenceNode(
                           new ActionNode(m_actions.WaitGateOrTimeout), // Success면 다음으로, Failure면 셀렉터 다음 브랜치로
                           new ActionNode(m_actions.SpecialPattern)     // QTE 본편 실행(기존 로직 그대로)
                       ),
                       // [실패 브랜치] 시간 초과로 실패했을 때의 연출/처리
                       new ActionNode(m_actions.OnGateFailed)           // 아래 3)에서 간단히 추가
                   ),
                   new ActionNode(m_actions.SetFalseDolly)
               );

               // ── 죽음 브랜치: HP<=0 이면 최우선 실행
               INode deathBranch = new SequenceNode(
                   new ActionNode(m_actions.CheckDeathCondition), // 죽음 조건
                   new ActionNode(m_actions.CleanupOnDeath),      // 연출/이동 정리
                   new ActionNode(m_actions.PlayDieAnim),         // 죽음 애니
                   new ActionNode(m_actions.StopTreeAction)       // 트리 중단
               );

               // ── 기존 루트 대체
               INode Result = new SelectorNode(
                   deathBranch,                     // ★ 최우선: 죽음 처리
                   new SelectorNode(                // 기존 흐름
                       specialFlow2,                // 조건 되면 패턴 실행
                       Move                         // 아니면 전투 이동/공격
                   )
               );

                   
        
       
               //생성된 노드를 기반으로 루트가  INode root = new SequenceNode 인 트리를 실행
               m_runner.SetTree(Result);
               m_runner.StartTree();
               
               // 기즈모 컴포넌트도 보스에게 붙여두면 편함
               if (!GetComponent<BossDebugGizmos>())
                   gameObject.AddComponent<BossDebugGizmos>();
       
               Debug.Log("Enter AttackState (BT started)");
               
    }
    
}
