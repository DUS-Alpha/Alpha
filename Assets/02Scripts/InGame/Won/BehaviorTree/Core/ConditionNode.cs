using System;
using UnityEngine;

public class ConditionNode : INode
{
    private readonly Func<bool> _condition;
    
    public ConditionNode(Func<bool> condition)
    {
        _condition = condition;
    }
    
    public NodeState Evaluate()
    {
        if (_condition == null)
            return NodeState.Failure;

        return _condition() ? NodeState.Success : NodeState.Failure;
    }

    public void Reset(){ }
}
