using System.Collections.Generic;


//Selector는 왼쪽(위)부터 검사해서 성공하는 갈래 하나만 선택
public class SelectorNode : INode
{
    private readonly List<INode> _children = new();
    private int m_currentIndex = 0; // ★ 진행상태 기억

    public SelectorNode(params INode[] children)
    {
        _children.AddRange(children);
    }

    public NodeState Evaluate()
    {
        while (m_currentIndex < _children.Count)
        {
            var s = _children[m_currentIndex].Evaluate();
            switch (s)
            {
                case NodeState.Failure:
                    m_currentIndex++;      // 다음 후보로
                    continue;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    m_currentIndex = 0;    // 성공했으면 다음 틱엔 처음부터
                    return NodeState.Success;
            }
        }
        m_currentIndex = 0;
        return NodeState.Failure;
    }

    //초기화
    public void Reset()
    {
        m_currentIndex = 0;
        foreach (var c in _children) c.Reset();
    }
}