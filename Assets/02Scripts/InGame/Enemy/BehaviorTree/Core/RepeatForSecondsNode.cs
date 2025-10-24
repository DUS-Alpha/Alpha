using UnityEngine;

public class RepeatForSecondsNode : INode
{
    private readonly INode _child;
    private readonly float _duration;
    private float _startTime;
    private bool _started;
    private NodeState _lastState = NodeState.Running;

    public RepeatForSecondsNode(float duration, INode child)
    {
        _duration = duration;
        _child = child;
    }

    public NodeState Evaluate()
    {
        if (!_started)
        {
            _startTime = Time.time;
            _started = true;
            Debug.Log($"[RepeatForSecondsNode] {_duration}초 동안 반복 실행 시작");
        }

        // 자식 노드 매 프레임 실행
        _lastState = _child.Evaluate();

        // 지정된 시간이 지났는지 확인
        if (Time.time - _startTime >= _duration)
        {
            Debug.Log($"[RepeatForSecondsNode] {_duration}초 종료 → 마지막 상태: {_lastState}");
            _started = false;
            return _lastState; // 🔹 마지막 자식 노드의 상태를 반환
        }

        // 아직 시간 남음 → 계속 실행
        return NodeState.Running;
    }

    public void Reset()
    {
        _started = false;
        _child?.Reset();
        _lastState = NodeState.Running;
    }
}