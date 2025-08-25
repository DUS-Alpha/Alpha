// BossLocomotionExecutor.cs  (Animator에 MoveX/MoveZ 세팅)

using UnityEngine;

public class BossLocomotionExecutor : MonoBehaviour {
    [Tooltip("[ Player ]")]
    public Transform target; 
    public Animator animator;
    public float rotSpeed = 12f, approachSpeed=3.2f, retreatSpeed=3.0f, orbitSideSpeed=3.0f, orbitFwdGain=2.0f;
    static readonly int MoveX = Animator.StringToHash("MoveX");
    static readonly int MoveZ = Animator.StringToHash("MoveZ");
    float sideSign = 1f, sideTimer;

    public void Tick(MoveMode mode, float idealRadius, float dt) {
        if (!target) return;
        Vector3 to = target.position - transform.position; to.y = 0;
        if (to.sqrMagnitude > 0.0001f) {
            var look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, rotSpeed * dt);
        }
        Vector3 fwd = transform.forward, right = transform.right, vel = Vector3.zero;

        switch (mode) {
            case MoveMode.Approach: vel = fwd * approachSpeed; break;
            case MoveMode.Retreat:  vel = -fwd * retreatSpeed; break;
            case MoveMode.Orbit:
                sideTimer -= dt;
                if (sideTimer <= 0f) { sideTimer = Random.Range(1.0f, 2.0f); if (Random.value < 0.5f) sideSign = -sideSign; }
                float dist = to.magnitude;
                float error = Mathf.Clamp(idealRadius - dist, -1f, 1f);
                vel = right * (orbitSideSpeed * sideSign) + fwd * (error * orbitFwdGain);
                break;
        }
        transform.position += vel * dt;
        // 블렌드 트리 입력
        float localX = Vector3.Dot(vel, right);
        float localZ = Vector3.Dot(vel, fwd);
        animator.SetFloat(MoveX, localX, 0.1f, dt);
        animator.SetFloat(MoveZ, localZ, 0.1f, dt);
    }
}