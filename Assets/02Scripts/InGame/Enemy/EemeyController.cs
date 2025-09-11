using System.Collections.Generic;
using UnityEngine;

public class EemeyController : MonoBehaviour
{
    private BehaviorTreeRunner m_runner;
    private BossActions m_actions;
    [SerializeField] Transform m_player; // 플레이어
    
    void Start()
    {
        m_runner  = GetComponent<BehaviorTreeRunner>() ?? gameObject.AddComponent<BehaviorTreeRunner>();
        m_actions = GetComponent<BossActions>()?? gameObject.AddComponent<BossActions>();
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
        
         // Todo : 대충 설명 + 지울 것
               // ★ 최소 골격 트리 (나중에 함수만 바꿔끼우면 됨)
               // 예: D에서 보여준 유틸 노드 감싸기 버전으로 교체
               // INode root = new SequenceNode(
               //     new TimeLimit(1.5f, new ActionNode(() => _actions.Approach())),
               //     new UntilSuccess(new ActionNode(() => _actions.Aim())),
               //     new SharedCooldown(() => _actions.BB, "burst", 2f,
               //         new ActionNode(() => _actions.Fire())
               //     ),
               //     new WaitSeconds(0.3f)
               // );
               
               //Todo : 대충 설명 + 지울 것
               //노드 시퀀스 생성
               // INode root = new SequenceNode(
               //     //필요시 데코레이터로 감싸서 추가하기
               //     new DebugWrap("Approach", new ActionNode(() => _actions.Approach())),
               //     new DebugWrap("Aim",      new ActionNode(() => _actions.Aim())),
               //     new DebugWrap("Fire",     new ActionNode(() => _actions.Fire()))
               // );
               //Todo : 대충 설명 + 지울 것
               // //IsMagazineEmpty가 true면 aim 하고 false면 fire 하기
               // INode SelectorRoot = 
               //     new SelectorNode(
               //     new DebugWrap("Empty→Aim", new SequenceNode(
               //         new ConditionNode(() => _actions.IsMagazineEmpty()),
               //         new ActionNode(() => _actions.Aim()),
               //         new ActionNode(() => _actions.Fire())
               //     )),
               //     new DebugWrap("NotEmpty→Fire", new SequenceNode(
               //         new ConditionNode(() => !_actions.dbgMagazineEmpty),
               //         new ActionNode(() => _actions.Fire()),
               //         new ActionNode(() => _actions.Approach())
               //     )) 
               // );
        

               var subNode = new List<INode>
               {
                   new ActionNode(m_actions.CloseRandomAni),
                   new ActionNode(m_actions.MiddleRandomAni),
                   new ActionNode(m_actions.FarRandomAni)
               };
               
           
               
               
               INode TestNode =
                   new SequenceNode(
                   new ActionNode(m_actions.Move),
                   new ActionNode(m_actions.SearchRange),
                   new SelectorNode(subNode.ToArray())
               );
               
               INode TestNode2 =
                   new SequenceNode(
                       
                       new ActionNode(m_actions.SpecialPattern)
                   );
               
               
               INode specialFlow = new SequenceNode(
                   new ActionNode(m_actions.ShowPatternPos),             // 장판 보여주기(선택)
                   new WaitUntilNode(() => m_actions.GateOpen),        // ★ G로 게이트 열릴 때까지 대기
                   new ActionNode(m_actions.HidePatternPos),             // 장판 감추기(선택)
                   new ActionNode(m_actions.SpecialPattern)             // ★ 기존 SpecialPattern 그대로 실행
               );

        
               
        
               
               // 또는 가중 선택 예시를 쓰고 싶다면:
               // INode root = new WeightedSelectorNode(new[]
               // {
               //     (node: (INode) new SharedCooldown(() => _actions.BB, "pA", 3f, new ActionNode(() => _actions.Fire())),   weight: 1f),
               //     (node: (INode) new SharedCooldown(() => _actions.BB, "pB", 5f, new ActionNode(() => _actions.Reload())), weight: 0.5f),
               // });
       
               //생성된 노드를 기반으로 루트가  INode root = new SequenceNode 인 트리를 실행
               m_runner.SetTree(specialFlow);
               m_runner.StartTree();
               
               // 기즈모 컴포넌트도 보스에게 붙여두면 편함
               if (!GetComponent<BossDebugGizmos>())
                   gameObject.AddComponent<BossDebugGizmos>();
       
               Debug.Log("Enter AttackState (BT started)");
               
    }
    
}
