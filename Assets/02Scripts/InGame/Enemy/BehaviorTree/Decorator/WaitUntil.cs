using System;

public class WaitUntil : INode
{
    private readonly Func<bool> _predicate;
    private readonly INode _child;

    public WaitUntil(Func<bool> predicate, INode child)
    {
        _predicate = predicate;
        _child = child;
    }

    public NodeState Evaluate()
    {
        // 조건이 만족될 때까지 대기
        if (_predicate == null || !_predicate()) return NodeState.Running;
        return _child != null ? _child.Evaluate() : NodeState.Success;
    }

    public void Reset()
    {
        _child?.Reset();
    }
}