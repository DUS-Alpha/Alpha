using System;
using UnityEngine;

public class ActionNode :INode
{
    private readonly Func<NodeState> _act;
    
    // 나중에 여기만 갈아끼우면 됨: _act에 원하는 행동(또는 조건체크)을 넣기
    public ActionNode(Func<NodeState> act) { _act = act; }
    
    public NodeState Evaluate() => _act != null ? _act() : NodeState.Failure;

    public void Reset() {Debug.Log("액션 노드 에서 나온 리셋 실행"); }
}
