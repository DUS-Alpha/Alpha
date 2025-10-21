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
        
        INode Testroot = new SelectorNode(
            new ActionNode(m_actions.CheckRangeCycle)
        );
        





        m_runner.SetTree(Testroot);
       
        m_runner.StartTree();
    }
}