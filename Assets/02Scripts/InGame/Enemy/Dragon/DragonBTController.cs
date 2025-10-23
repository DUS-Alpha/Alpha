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
            new WaitNode(1f, new ActionNode(m_actions.CheckRangeCycle)),   // 거리 측정
            new SelectorNode(                                   // 거리별 패턴 선택
                new SequenceNode(                               // 근거리 공격
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Close),
                    new ActionNode(m_actions.MeleeAttack)
                ),
                new SequenceNode(                               // 중거리 브레스
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Mid),
                    new ActionNode(m_actions.DoBreatheFire)
                ),
                new SequenceNode(                               // 원거리 불덩이
                    new ConditionNode(() => m_actions.checkDistanceSetting._type == DistanceCheckType.Far),
                    new ActionNode(m_actions.AttackFireBall)
                )
            ) //, 여기에 잠깐 대하기는 데코레이터 구현
        );
        
        
        INode root = new SelectorNode(
            new SequenceNode(
                new ConditionNode(() => m_actions.currentDeathSettings.isDead),//isDead가 True라면  Sucess
                new ActionNode(m_actions.Death)
            ),new ActionNode(m_actions.CheckHitReaction),
            AttackLogic
        );
        





        m_runner.SetTree(root);
       
        m_runner.StartTree();
    }
}