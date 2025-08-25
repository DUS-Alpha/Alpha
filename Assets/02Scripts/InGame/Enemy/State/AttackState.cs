using UnityEngine;

public class AttackState : IEnemyState
{
    private BehaviorTreeRunner _runner;
    private BossActions _actions;
    private CombatMover _combatMover;
    
    public void EnterState(EnemyStateManager enemy)
    {
         // 러너/액션 컴포넌트 확보(없으면 자동 추가)
         _runner  = enemy.GetComponent<BehaviorTreeRunner>() ?? enemy.gameObject.AddComponent<BehaviorTreeRunner>();
         _actions = enemy.GetComponent<BossActions>()       ?? enemy.gameObject.AddComponent<BossActions>();
         _combatMover = enemy.GetComponent<CombatMover>()       ?? enemy.gameObject.AddComponent<CombatMover>();

               
               // ★ 추가: 블랙보드 생성 및 기본 연결
               var bb = new Blackboard(enemy.gameObject)
               {
                   // 필요하면 여기서 초기화 (예: Target을 EnemyStateManager에서 끌어와 연결)
                   // EnemyStateManager에 target이 있다면:
                   // Target = enemy.target
                   Target = enemy.Target, // ← 지금은 비워둠. 나중에 네가 연결
               };
               _actions.SetBlackboard(bb);
        
       
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
               
               //노드 시퀀스 생성
               INode root = new SequenceNode(
                   //필요시 데코레이터로 감싸서 추가하기
                   new DebugWrap("Approach", new ActionNode(() => _actions.Approach())),
                   new DebugWrap("Aim",      new ActionNode(() => _actions.Aim())),
                   new DebugWrap("Fire",     new ActionNode(() => _actions.Fire()))
               );

               //IsMagazineEmpty가 true면 aim 하고 false면 fire 하기
               INode SelectorRoot = 
                   new SelectorNode(
                   new DebugWrap("Empty→Aim", new SequenceNode(
                       new ConditionNode(() => _actions.IsMagazineEmpty()),
                       new ActionNode(() => _actions.Aim()),
                       new ActionNode(() => _actions.Fire())
                   )),
                   new DebugWrap("NotEmpty→Fire", new SequenceNode(
                       new ConditionNode(() => !_actions.dbgMagazineEmpty),
                       new ActionNode(() => _actions.Fire()),
                       new ActionNode(() => _actions.Approach())
                   )) 
               );

        
               INode SelectorNode = new SelectorNode(
                    new ActionNode(() => _actions.Fire())
               );

               INode TestNode =
                   new SequenceNode(
                   new ActionNode(() => _combatMover.CombatMove(bb.Target))
               );
               
        
               
               // 또는 가중 선택 예시를 쓰고 싶다면:
               // INode root = new WeightedSelectorNode(new[]
               // {
               //     (node: (INode) new SharedCooldown(() => _actions.BB, "pA", 3f, new ActionNode(() => _actions.Fire())),   weight: 1f),
               //     (node: (INode) new SharedCooldown(() => _actions.BB, "pB", 5f, new ActionNode(() => _actions.Reload())), weight: 0.5f),
               // });
       
               //생성된 노드를 기반으로 루트가  INode root = new SequenceNode 인 트리를 실행
               _runner.SetTree(TestNode);
               _runner.StartTree();
               
               // 기즈모 컴포넌트도 보스에게 붙여두면 편함
               if (!enemy.GetComponent<BossDebugGizmos>())
                   enemy.gameObject.AddComponent<BossDebugGizmos>();
       
               Debug.Log("Enter AttackState (BT started)");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemy.TransitionToState(new IdleState());
        }
    }

    public void ExitState(EnemyStateManager enemy)
    {
        if (_runner != null) _runner.StopTree();
        Debug.Log("Exit AttackState (BT stopped)");
    }
}
