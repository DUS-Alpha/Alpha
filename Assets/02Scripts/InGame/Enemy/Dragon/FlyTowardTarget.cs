using UnityEngine;

public class FlyTowardTarget : MonoBehaviour
{
    public NodeState Execute(Blackboard bb,FlySettings settings)
    {
        if (bb.Target == null || settings.hoverCenter == null)
            return NodeState.Failure;

        // 1. 위치 추적
        Vector3 target = new Vector3(bb.Target.position.x, bb.Target.position.y + settings.hoverHeight, bb.Target.position.z);
        Vector3 direction = target - transform.position;
        Vector3 MyTransform = new Vector3(transform.position.x, bb.Target.position.y + settings.hoverHeight, transform.position.z);

        // Y축 평면 회전
        Vector3 flatDirection = direction;
        flatDirection.y = 0f;
        if (flatDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * settings.turnSpeed);
        }

        // 이동
        transform.position = Vector3.MoveTowards(MyTransform, bb.Target.position, Time.deltaTime * settings.moveSpeed);
        
        return NodeState.Success; 
    }

    
}
