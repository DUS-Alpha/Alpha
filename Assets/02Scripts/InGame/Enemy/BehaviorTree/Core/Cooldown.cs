using UnityEngine;

public class Cooldown : INode
{
    private readonly INode _child;
    private readonly float _seconds;
    private float _readyTime = 0f;

    public Cooldown(float seconds, INode child)
    {
        _seconds = seconds;
        _child = child;
    }

    public NodeState Evaluate()
    {
        if (Time.time < _readyTime) return NodeState.Failure; // 아직 쿨타임
        var s = _child != null ? _child.Evaluate() : NodeState.Success;
        if (s == NodeState.Success)
            _readyTime = Time.time + _seconds;
        return s;
    }

    public void Reset()
    {
        _readyTime = 0f;
        _child?.Reset();
    }
}