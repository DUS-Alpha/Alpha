using System;
using System.Collections.Generic;
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
    

     
        

        /*INode root = new SelectorNode(
            new SequenceNode(
                new ActionNode(m_actions.CheckDeath),
                new ActionNode(m_actions.Fall)
            ),
            new ActionNode(m_actions.CheckHitReaction),
            new SequenceNode(
                new ActionNode(m_actions.CheckBreath),
                new ActionNode(m_actions.BreatheFire)
            ),
            new SequenceNode(
                new ActionNode(m_actions.FlyTowardTarget),
                new ActionNode(m_actions.FireballAttack)
            )
        );*/


            
      
        
        INode AttackLogic = new SequenceNode(
            new ActionNode(m_actions.CheckRangeCycle),   // 거리 측정
            new SelectorNode(                                   // 거리별 패턴 선택
                new SequenceNode(                               // 근거리 공격
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Close),
                    new ActionNode(m_actions.MeleeAttack),
                    new WaitSecondsNode(2.5f)
                ),
                new SequenceNode(                               // 중거리 브레스
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Mid),
                    new ActionNode(m_actions.DoBreatheFire),
                    new WaitSecondsNode(2.5f)
                    
                ),
                new SequenceNode(                               // 원거리 불덩이
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Far),
                    new WeightedSelectorNode()
                        .AddNode(new ActionNode(m_actions.BodyAttack), 0.7f)    // 70%
                        .AddNode(new ActionNode(m_actions.AttackFireBall), 0.3f) // 30%
                    ,new WaitSecondsNode(5f)
                )
            ) //, 여기에 잠깐 대하기는 데코레이터 구현
        );
        
        
     
        
        INode root2 = new ParallelNode(
                new SequenceNode(
                    new ConditionNode(() => m_actions.currentDeathSettings.isDead),
                    new ActionNode(m_actions.Death)
            ),
                new SequenceNode(new ActionNode(m_actions.LookAtAndWalk),
                    AttackLogic
                    )
        );
        
        
       
        INode root3 = new SelectorNode(
            new ActionNode(m_actions.BodyAttack)
        );
        

        
        





       m_runner.SetTree(root2);
       
        m_runner.StartTree();
    }
}