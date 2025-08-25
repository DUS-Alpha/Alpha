using UnityEngine;


//행동이 너무 오래 걸리면 끊기
//시간 초과시 실패  나중에 성공으로도 바꿀 수 있지 않을까 ?
public class TimeLimit : INode
{
    private readonly INode _child;
    private readonly float _seconds;
    private float _end = -1f;

    public TimeLimit(float seconds, INode child)
    {
        _seconds = seconds;
        _child = child;
    }

    public NodeState Evaluate()
    {
        if (_end < 0f) _end = Time.time + _seconds;

        if (Time.time > _end)
        {
            _child.Reset();
            _end = -1f;
            return NodeState.Failure;
        }

        var s = _child.Evaluate();
        if (s == NodeState.Success || s == NodeState.Failure)
        {
            _end = -1f;
        }
        return s;
    }

    public void Reset()
    {
        _end = -1f;
        _child.Reset();
    }
}