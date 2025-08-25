
//“준비/탐색만 하고 넘어가라”
//항상 실패
public class Failer : INode
{
    private readonly INode _child;
    public Failer(INode child) { _child = child; }

    public NodeState Evaluate()
    {
        _child.Evaluate();
        return NodeState.Failure;
    }

    public void Reset() => _child.Reset();
}