

//미리 설정해둔 임의의 수만큼 자식 노드 반복
public class Repeat : INode
{
    private readonly INode _child;
    private readonly int _times;
    private int _count = 0;

    public Repeat(int times, INode child)
    {
        _times = times;
        _child = child;
    }

    public NodeState Evaluate()
    {
        var s = _child.Evaluate();
        if (s == NodeState.Running) return NodeState.Running;

        if (s == NodeState.Success)
        {
            _count++;
            _child.Reset();
            if (_count >= _times)
            {
                _count = 0;
                return NodeState.Success;
            }
            return NodeState.Running;
        }

        // 실패 시 반복 중단(필요하면 정책 바꿔도 됨)
        _count = 0;
        _child.Reset();
        return NodeState.Failure;
    }

    public void Reset()
    {
        _count = 0;
        _child.Reset();
    }
}