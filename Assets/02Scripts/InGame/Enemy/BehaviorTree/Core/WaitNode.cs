

using UnityEngine;

public class WaitNode :INode
{    
    private INode _child;
    private float _waitTime;
    private float _startTime;
    private bool _started = false;
    
    
    public WaitNode(float waitTime, INode child)
    {
        _waitTime = waitTime;
        _child = child;
    }
    
    public NodeState Evaluate()
    {
        
        if (!_started)
        {
            _startTime = Time.time;
            _started = true;
            Debug.Log($"대기 시작:{_waitTime}초 대기");
            return NodeState.Running;
        }

        if (Time.time - _startTime >= _waitTime)
        {
            Debug.Log("대기 종료 , 노드 실행");
            NodeState result = _child.Evaluate();
            if (result != NodeState.Running)
                _started = false; // 다음 실행을 위해 초기화
            return result;
        }

        return NodeState.Running;
    }

    public void Reset()
    {
        _started = false;
        if (_child != null)
            _child.Reset();
    }
}
