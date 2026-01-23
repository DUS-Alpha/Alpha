using System.Collections.Generic;
using UnityEngine;

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
    

    public Blackboard(GameObject owner)
    {
        Owner = owner;
        OwnerTransform = owner.transform;
    }

   
}