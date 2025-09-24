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
        var flyToTarget = new ActionNode(m_actions.FlyTowardTarget);
        var breatheFire = new ActionNode(m_actions.BreatheFire);

        var flySequence = new SequenceNode(
            ascendToHover,
            flyToTarget,
            breatheFire
        );

        // ── 루트 트리 구성
        INode root = new SelectorNode(
            deathBranch,
            flyToTarget
        );

        m_runner.SetTree(root);
        m_runner.StartTree();
    }
}