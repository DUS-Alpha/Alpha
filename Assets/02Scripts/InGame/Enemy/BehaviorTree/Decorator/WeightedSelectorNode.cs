using System.Collections.Generic;
using UnityEngine;

public class WeightedSelectorNode : INode
{
    private readonly List<INode> _children = new();
    private readonly List<float> _weights = new();
    private int _currentIndex = -1;

    public WeightedSelectorNode((INode node, float weight)[] entries)
    {
        foreach (var e in entries)
        {
            _children.Add(e.node);
            _weights.Add(Mathf.Max(0f, e.weight));
        }
    }

    public NodeState Evaluate()
    {
        // 현재 실행 중인 자식이 있으면 그걸 계속 틱
        if (_currentIndex >= 0)
        {
            var s = _children[_currentIndex].Evaluate();
            if (s == NodeState.Running) return NodeState.Running;

            // 종료되면 초기화하고 다음 선택
            _children[_currentIndex].Reset();
            _currentIndex = -1;
            return s; // Success/Failure 그대로 위로
        }

        // 새로 선택
        float total = 0f;
        for (int i = 0; i < _weights.Count; i++) total += _weights[i];
        if (total <= 0f) return NodeState.Failure;

        float pick = Random.value * total;
        float acc = 0f;
        for (int i = 0; i < _children.Count; i++)
        {
            acc += _weights[i];
            if (pick <= acc)
            {
                _currentIndex = i;
                break;
            }
        }

        // 첫 Evaluate는 다음 프레임에 실행되도록 Running
        return NodeState.Running;
    }

    public void Reset()
    {
        if (_currentIndex >= 0)
            _children[_currentIndex].Reset();
        _currentIndex = -1;
        foreach (var c in _children) c.Reset();
    }
}