

//자식 노드 값 상관 없이 무조건 성공 반환
//부수효과가 실패해도 시퀀스를 끊지 않을 때
public class Succeeder : INode
{
    private readonly INode _child;
    public Succeeder(INode child) { _child = child; }

    public NodeState Evaluate()
    {
        _child.Evaluate();
        return NodeState.Success;
    }

    public void Reset() => _child.Reset();
}