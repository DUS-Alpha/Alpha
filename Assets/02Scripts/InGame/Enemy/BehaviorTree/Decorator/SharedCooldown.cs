using System;

public class SharedCooldown : INode
{
    private readonly Func<Blackboard> _getBB;
    private readonly string _key;
    private readonly float _seconds;
    private readonly INode _child;

    public SharedCooldown(Func<Blackboard> getBlackboard, string key, float seconds, INode child)
    {
        _getBB = getBlackboard;
        _key = key;
        _seconds = seconds;
        _child = child;
    }

    public NodeState Evaluate()
    {
        var bb = _getBB?.Invoke();
        if (bb == null) return NodeState.Failure;

        if (bb.IsOnCooldown(_key)) return NodeState.Failure;

        var s = _child.Evaluate();
        if (s == NodeState.Success)
            bb.StartCooldown(_key, _seconds);

        return s;
    }

    public void Reset() => _child.Reset();
}