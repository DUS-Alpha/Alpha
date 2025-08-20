
//성공할 때까지 재시도 
//Success 될 때까지 Running


public class UntilSuccess : INode
{
    private readonly INode _child;
    public UntilSuccess(INode child) { _child = child; }

    public NodeState Evaluate()
    {
        var s = _child.Evaluate();
        if (s == NodeState.Success) return NodeState.Success;
        if (s == NodeState.Failure || s == NodeState.Running) return NodeState.Running;
        return NodeState.Running;
    }

    public void Reset() => _child.Reset();
}