using UnityEngine;

public class DragonBossActions : MonoBehaviour
{
    public Animator animator;
    public Transform hoverCenter;
    public float hoverHeight = 5f;
    public float moveSpeed = 5f;
    public float turnSpeed = 3f;

    private Blackboard BB;
    [SerializeField]private bool _attackStarted;

    public void SetBlackboard(Blackboard bb)
    {
        BB = bb;
    }

    public NodeState AscendToHoverHeight()
    {
        if (hoverCenter == null) return NodeState.Failure;

        float targetY = hoverCenter.position.y + hoverHeight;
        Vector3 current = transform.position;
        Vector3 target = new Vector3(current.x, targetY, current.z);

        transform.position = Vector3.MoveTowards(current, target, Time.deltaTime * moveSpeed);

        if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
        {
            print("성공");
            return NodeState.Success;
        }

        

        return NodeState.Running;
    }

    public NodeState FlyTowardTarget()
    {
        if (BB?.Target == null || hoverCenter == null) return NodeState.Failure;

        Vector3 target = new Vector3(BB.Target.position.x, hoverCenter.position.y + hoverHeight, BB.Target.position.z);
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;

        Vector3 flatDirection = direction;
        flatDirection.y = 0f;
        if (flatDirection != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

        /*if (distance < 1.5f)
            return NodeState.Success;*/

        return NodeState.Running;
    }

    public NodeState HoverIdle()
    {
        if (hoverCenter == null) return NodeState.Failure;

        float sin = Mathf.Sin(Time.time * 2f) * 0.5f;
        Vector3 target = new Vector3(hoverCenter.position.x, hoverCenter.position.y + hoverHeight + sin, hoverCenter.position.z);

        if (BB?.Target != null)
        {
            Vector3 lookDir = BB.Target.position - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
            }
        }

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 2f);

        return NodeState.Running;
    }

    public NodeState BreatheFire()
    {
        if (!_attackStarted)
        {
            animator.SetTrigger("FireBreath");
            _attackStarted = true;
            return NodeState.Running;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fly Breathe Fire") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            print("애니메이션 확인에 들어옴");
            _attackStarted = false;
            return NodeState.Success;
        }
        print("계속 시전중");
        return NodeState.Running;
    }

    public NodeState CheckDeath()
    {
        // 체력 시스템이 있으면 여기에 추가
        return NodeState.Failure;
    }
} 
