using System;

public class ConditionNode : INode
{
    private readonly Func<bool> _predicate;
    public ConditionNode(Func<bool> predicate) { _predicate = predicate; }

    public NodeState Evaluate()
    {
        return _predicate != null && _predicate() ? NodeState.Success : NodeState.Failure;
    }

    public void Reset() { }
}