using System.Collections.Generic;
using UnityEngine;

// 가중치 기반으로 자식 노드 하나를 선택해 실행하는 Selector 노드
public class WeightedSelectorNode : INode
{
    private class WeightedNode
    {
        public INode node; // 실행 할 자식 노드
        public float weight; // 실행 할 가중치
    }
    
    //가중치를 넣어놓은 자식노드들 목록
    private List<WeightedNode> _nodes = new List<WeightedNode>();
    
    //리스트에 넣기 위한 Add함수 
    public WeightedSelectorNode AddNode(INode node, float weight)
    {
        _nodes.Add(new WeightedNode { node = node, weight = weight });
        return this;
    }

    //실제 실행함수 
    public NodeState Evaluate()
    {
        if (_nodes.Count == 0)
            return NodeState.Failure;

        // 가중치 총합
        float totalWeight = 0f;
        foreach (var n in _nodes)
            totalWeight += n.weight;

        // 랜덤 시드
        float rand = Random.value * totalWeight;

        // 4) 각 노드의 weight를 순서대로 빼며 rand가 작아지는 지점의 노드를 선택
        //    예: weights [0.7,0.3]이면 rand가 0~0.7이면 첫번째, 0.7~1.0이면 두번째
        foreach (var n in _nodes)
        {
            if (rand < n.weight)
                return n.node.Evaluate();

            rand -= n.weight;
        }

        return NodeState.Failure;
    }

    public void Reset()
    {
        foreach (var n in _nodes)
            n.node.Reset();
    }
}
