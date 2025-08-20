using UnityEngine;


//잠깐 텀/연출
//n초 대기 후 Success
public class WaitSeconds : INode
{
    private readonly float _seconds;
    private float _start = -1f;

    public WaitSeconds(float seconds) { _seconds = seconds; }

    public NodeState Evaluate()
    {
        if (_start < 0f) _start = Time.time;
        if (Time.time - _start < _seconds) return NodeState.Running;
        return NodeState.Success;
    }

    public void Reset() { _start = -1f; }
}