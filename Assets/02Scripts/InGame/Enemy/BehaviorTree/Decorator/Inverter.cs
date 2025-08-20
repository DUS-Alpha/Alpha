
//자식 노드의 반환 값을 반대로 바꾸는 노드. 성공 -> 실패, 실패 -> 성공을 반환.
//기존 조건의 반대가 필요할 때
public class Inverter : INode
{
    private readonly INode _child;
    public Inverter(INode child) { _child = child; }

    public NodeState Evaluate()
    {
        var s = _child.Evaluate();
        if (s == NodeState.Success) return NodeState.Failure;
        if (s == NodeState.Failure) return NodeState.Success;
        return NodeState.Running;
    }

    public void Reset() => _child.Reset();
}