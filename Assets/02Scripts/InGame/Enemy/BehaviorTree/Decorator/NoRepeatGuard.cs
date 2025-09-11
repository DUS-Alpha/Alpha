using UnityEngine;

public class NoRepeatGuard : INode
{
    private readonly INode _child;
    private readonly string _key;
    private static string _lastKey;

    public NoRepeatGuard(string key, INode child)
    {
        _key = key; _child = child;
    }

    public NodeState Evaluate()
    {
        if (_lastKey == _key)
        {
            Debug.Log("똑같은거 두번함");
            return NodeState.Failure; // 같은 키 연속이면 건너뛰기
        }
        
        var s = _child.Evaluate();//현재 노드의 상태
        if (s == NodeState.Success || s == NodeState.Running)
            _lastKey = _key; // 이번에 이 키를 탔다는 걸 기록
        return s;
    }

    public void Reset() { /* 필요시 구현 */ }
}