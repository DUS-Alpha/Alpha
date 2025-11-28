using UnityEngine;

public class WaitSecondsNode : INode
{
    private float waitTime;
    private float timer = 0f;

    public WaitSecondsNode(float time)
    {
        waitTime = time;
    }

    public NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            timer = 0f; // 다음 실행 시 초기화
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void Reset()
    {
    }
}