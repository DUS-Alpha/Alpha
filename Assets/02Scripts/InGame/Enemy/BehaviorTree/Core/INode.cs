using UnityEngine;

public interface INode
{
    NodeState Evaluate(); // 1틱 수행
    void Reset();         // 트리/서브트리 재시작 시 초기화
}
