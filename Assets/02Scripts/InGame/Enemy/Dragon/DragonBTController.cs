using System.Collections.Generic;
using UnityEngine;

public class DragonBTController : MonoBehaviour
{
    private BehaviorTreeRunner m_runner;
    private DragonBossActions m_actions;
    [SerializeField] private Transform m_player;

    void Start()
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

        // ── 행동 노드들 구성
        var idleHover = new ActionNode(m_actions.HoverIdle);
        var deathBranch = new ActionNode(m_actions.CheckDeath);
        var ascendToHover = new ActionNode(m_actions.AscendToHoverHeight);
        var flyToTargetAttack = new ActionNode(m_actions.FlyTowardTargetAndFire);
        var breatheFire = new ActionNode(m_actions.BreatheFire);
        var fireballAttack = new ActionNode(m_actions.FireballAttack);
        var flyToTarget = new ActionNode(m_actions.FlyTowardTarget);

        var flySequence = new SequenceNode(
            ascendToHover,
            flyToTarget,
            breatheFire
        );

        INode root = new SelectorNode(
            new SequenceNode(                     // 죽음 처리 시퀀스
                new ActionNode(m_actions.CheckDeath), 
                new ActionNode(m_actions.Fall)
            ),
            new ActionNode(m_actions.CheckHitReaction), 
            new ActionNode(m_actions.CheckBreath),         
            new ActionNode(m_actions.BreatheFire),         
            new SequenceNode(
                new ActionNode(m_actions.FlyTowardTarget),
                new ActionNode(m_actions.FireballAttack)
            )
        );




        m_runner.SetTree(root);
       
        m_runner.StartTree();
    }
}