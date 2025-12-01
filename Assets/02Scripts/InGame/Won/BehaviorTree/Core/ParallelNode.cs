using System.Collections.Generic;
using UnityEngine;

public class ParallelNode : INode
{
    private readonly List<INode> _children;
    private readonly bool _requireAllSuccess; // true면 모든 자식이 Success일 때 Success, false면 하나라도 Success면 Success

    public ParallelNode(bool requireAllSuccess = false, params INode[] children)
    {
        _requireAllSuccess = requireAllSuccess;
        _children = new List<INode>(children);
    }
    
    public ParallelNode(params INode[] children)
    {
        _requireAllSuccess = false;
        _children = new List<INode>(children);
    }

    public NodeState Evaluate()
    {
        bool anyRunning = false;
        bool anySuccess = false;

        foreach (var child in _children)
        {
            var state = child.Evaluate();

            if (state == NodeState.Running)
                anyRunning = true;
            else if (state == NodeState.Success)
                anySuccess = true;
        }

        // 모든 자식이 성공해야 성공하는 모드
        if (_requireAllSuccess)
        {
            if (_children.TrueForAll(c => c.Evaluate() == NodeState.Success))
                return NodeState.Success;
            if (anyRunning)
                return NodeState.Running;
            return NodeState.Failure;
        }
        // 하나라도 성공하면 성공
        else
        {
            if (anySuccess)
                return NodeState.Success;
            if (anyRunning)
                return NodeState.Running;
            return NodeState.Failure;
        }
    }

    public void Reset()
    {
        foreach (var c in _children)
            c.Reset();
    }
}