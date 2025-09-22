using UnityEngine;

//노드 디버그용으로 사용 어떤 노드인지 확인하기 위해 
public class DebugWrap : INode
{
    private readonly INode _child;
    private readonly string _name;

    public DebugWrap(string name, INode child)
    {
        _name = name;
        _child = child;
    }

    public NodeState Evaluate()
    {
        var s = _child.Evaluate();
        Debug.Log($"[BT] {_name} -> {s}");
        return s;
    }

    public void Reset()
    {
        _child.Reset();
    }
}