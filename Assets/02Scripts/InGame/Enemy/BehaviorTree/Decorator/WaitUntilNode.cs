// 간단 대기 노드
public class WaitUntilNode : INode
{
    private readonly System.Func<bool> cond;
    public WaitUntilNode(System.Func<bool> cond) { this.cond = cond; }
    public NodeState Evaluate() => cond() ? NodeState.Success : NodeState.Running;
    public void Reset() {}
}