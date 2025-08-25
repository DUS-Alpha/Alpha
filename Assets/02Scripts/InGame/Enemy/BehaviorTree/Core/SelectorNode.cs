using System.Collections.Generic;


//Selector는 왼쪽부터 검사해서 성공하는 갈래 하나만 선택
public class SelectorNode : INode
{
    private readonly List<INode> _children = new();
    private int _currentIndex = 0; // ★ 진행상태 기억

    public SelectorNode(params INode[] children)
    {
        _children.AddRange(children);
    }

    public NodeState Evaluate()
    {
        while (_currentIndex < _children.Count)
        {
            var s = _children[_currentIndex].Evaluate();
            switch (s)
            {
                case NodeState.Failure:
                    _currentIndex++;      // 다음 후보로
                    continue;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    _currentIndex = 0;    // 성공했으면 다음 틱엔 처음부터
                    return NodeState.Success;
            }
        }
        _currentIndex = 0;
        return NodeState.Failure;
    }

    public void Reset()
    {
        _currentIndex = 0;
        foreach (var c in _children) c.Reset();
    }
}