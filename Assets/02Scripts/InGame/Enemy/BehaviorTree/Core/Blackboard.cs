using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// BT 노드/액션들이 함께 쓰는 공유 컨텍스트.
/// 필요하면 자유롭게 필드를 더 추가해도 됨.
/// </summary>
public class Blackboard
{
    // 참조들
    public GameObject Owner;         // 보스 본체
    public Transform OwnerTransform; // 보스 Transform
    public Transform Target;         // 현재 타깃(플레이어 등)
    public NavMeshAgent Agent;       // 이동 에이전트(있으면)
    
    // (선행 예비) 쿨다운 키 공유용 — 지금은 쓰지 않아도 됨
    private readonly Dictionary<string, float> _cooldowns = new();

    public Blackboard(GameObject owner)
    {
        Owner = owner;
        OwnerTransform = owner.transform;
        if (owner.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogWarning("Blackboard could not find NavMeshAgent");
        }
        Agent = owner.GetComponent<NavMeshAgent>();
    }

   
}