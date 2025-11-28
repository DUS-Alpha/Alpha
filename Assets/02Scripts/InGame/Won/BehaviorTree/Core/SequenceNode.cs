using System.Collections.Generic;
using UnityEngine;

//and 역활 조건 모두 성공시  Running,과 Success를 반환 (Running이 우선 순위 높음 )
public class SequenceNode : INode
{
    private readonly List<INode> _children = new();
    private int _currentIndex = 0; // 진행상태 기억

    //생성자
    //SequenceNode가 생성될 때 자식 노드들을 받아서 리스트에 저장하는 것
    public SequenceNode(params INode[] children) { _children.AddRange(children); }

    public NodeState Evaluate()
    {
        while (_currentIndex < _children.Count)
        {
            var s = _children[_currentIndex].Evaluate();
            switch (s)
            {
                case NodeState.Failure:
                    _currentIndex = 0;
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    _currentIndex++;      // 다음 단계로
                    continue;
            }
        }
        _currentIndex = 0;
        return NodeState.Success;
    }

    public void Reset()
    {
        _currentIndex = 0;
        foreach (var c in _children) c.Reset();
        Debug.Log("시퀀스노드에 나온 리셋 실행");
    }
}