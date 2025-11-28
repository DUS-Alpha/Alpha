using UnityEngine;

public class DragonBTController : MonoBehaviour
{
    private BehaviorTreeRunner m_runner;
    private DragonBossActions m_actions;
    [SerializeField] private Transform m_player;

    private void Awake()
    {
        m_runner = GetComponent<BehaviorTreeRunner>();
        m_actions = GetComponent<DragonBossActions>();

        var bb = new Blackboard(gameObject)
        {
            Target = m_player,
            Owner = gameObject,
            OwnerTransform = transform,
        };
        m_actions.SetBlackboard(bb);
    }

    void Start()
    {
    
        INode AttackLogic = new SequenceNode(
            new ActionNode(m_actions.CheckRangeCycle),   // 거리 측정
            new SelectorNode(                                   // 거리별 패턴 선택
                new SequenceNode(                               // 근거리 공격
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Close),
                    new WeightedSelectorNode()
                        .AddNode(new ActionNode(m_actions.MeleeAttack), 0.5f)    // 50%
                        .AddNode(new ActionNode(m_actions.BiteAttack), 0.5f), // 50%
                    new WaitSecondsNode(2f)
                ),
                new SequenceNode(                               // 중거리 브레스
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Mid),
                    new WeightedSelectorNode()
                        .AddNode(new ActionNode(m_actions.DoBreatheFire2), 0.9f)    // 50%
                        .AddNode(new ActionNode(m_actions.Roar), 0.1f), // 50%
                    new WaitSecondsNode(2f)
                ),
                new SequenceNode(                               // 원거리 불덩이
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Far)
                        ,new ActionNode(m_actions.Run) 
                    ,new WaitSecondsNode(2f)
                )
            ) //, 여기에 잠깐 대하기는 데코레이터 구현
        );
        
              
        INode FlyFireBallLogic = new SequenceNode(
            new ActionNode(m_actions.Takeoff),
            new ActionNode(m_actions.Flyfrieball),
            new ActionNode(m_actions.Landing)
        );
        INode FlyFireBallOneShot = new OneShotNode(
            new SequenceNode(
                new ConditionNode(() => m_actions.IsLowHp()),   // HP 50% 이하일 때만 실행
                FlyFireBallLogic
            )
        );
     
        
        INode root2 = new ParallelNode(
                new SequenceNode(
                    new ConditionNode(() => m_actions.currentDeathSettings.isDead),
                    new ActionNode(m_actions.Death)
            ),
                new SelectorNode(   
                    FlyFireBallOneShot,
                        new SequenceNode(
                            new ActionNode(m_actions.LookAtAndWalk),
                            AttackLogic
                        )
                    )
              
                
        
        );

        INode testroot = new ActionNode(m_actions.Flyfrieball);
        
        
        


       m_runner.SetTree(root2);
       
       m_runner.StartTree();
    }
}