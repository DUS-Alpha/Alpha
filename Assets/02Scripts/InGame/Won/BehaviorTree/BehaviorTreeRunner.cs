using UnityEngine;
using System;

public class BehaviorTreeRunner : MonoBehaviour
{
    private BehaviorTree _tree; // 루트 노드를 저장할 변수 
    private Func<INode> _builder;
    public bool IsRunning { get; private set; }

    public void SetTree(INode root)
    {
        _tree = new BehaviorTree(root); // 외부에서 넘겨준 root를 여기 보관
    }

    /*// 빌더를 넘겨두고 StartTree() 시점에 만들고 싶을 때 사용
    public void SetBuilder(Func<INode> builder)
    {
        _builder = builder;
    }*/

    public void StartTree()
    {
        /*if (_tree == null)
        {
            if (_builder != null) _tree = new BehaviorTree(_builder());
            else _tree = new BehaviorTree(new ActionNode(() => NodeState.Success)); // 기본 더미
        }*/
        _tree.Reset();
        // (_tree.Reset() → 내부적으로 _root.Reset() 호출)
        IsRunning = true;
    }

    public void StopTree()
    {
        IsRunning = false;
    }

    private void Update()
    {
        if (!IsRunning || _tree == null) return;
        // 매 프레임 트리 실행
        // (_tree.Tick() → 내부적으로 _root.Evaluate() 호출)
        _tree.Tick();
    }
}