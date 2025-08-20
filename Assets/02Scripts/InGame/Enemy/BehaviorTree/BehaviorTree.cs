public class BehaviorTree
{
    private readonly INode _root;
    public BehaviorTree(INode root) { _root = root; }
    public void Reset() => _root.Reset();
    /* = public void Reset() {
         _root.Reset();
    } */
    public NodeState Tick() => _root.Evaluate();
}