public class OneShotNode : INode
{
    private INode child;
    private bool hasExecuted = false;

    public OneShotNode(INode childNode)
    {
        child = childNode;
    }

    public NodeState Evaluate()
    {
        if (hasExecuted)
            return NodeState.Failure;  // 한 번 실행 이후에는 항상 실패로 간주 (다시 실행되지 않음)

        var result = child.Evaluate();

        if (result == NodeState.Success)
            hasExecuted = true;

        return result;
    }

    public void Reset()
    {
        
    }
}