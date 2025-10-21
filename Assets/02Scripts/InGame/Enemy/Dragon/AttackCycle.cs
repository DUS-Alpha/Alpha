using UnityEngine;

public class AttackCycle : MonoBehaviour
{
    
    public NodeState FireballAttack(Blackboard BB,AttackSetting attackCycle)
    {
        if (BB?.Target == null || attackCycle.fireballPrefab == null || attackCycle.firePoints.Length == 0)
            return NodeState.Failure;

        attackCycle._fireTimer += Time.deltaTime;

        if (attackCycle._fireTimer >= attackCycle.fireInterval)
        {
            attackCycle._fireTimer = 0f;

            
            Transform point = GetNextFirePoint(attackCycle);
            // 풀에서 꺼내오기
            Vector3 dir = (BB.Target.position - point.position).normalized;
            GameObject fireball = PoolManager.Instance.Spawn(attackCycle.fireballPrefab, point.position, Quaternion.LookRotation(dir));
            
            fireball.GetComponent<PooledProjectile>().Launch(dir, attackCycle.fireSpeed, false);
            
            Debug.Log("🔥 Fireball launched!");
        }
        
        return NodeState.Success;
    }
    
    private Transform GetNextFirePoint(AttackSetting attackCycle)
    {
        if (attackCycle.firePoints == null || attackCycle.firePoints.Length == 0)
            return transform; // 기본값: 자기 자신


        Transform point = attackCycle.firePoints[attackCycle._firePointIndex];
        attackCycle._firePointIndex = (attackCycle._firePointIndex + 1) % attackCycle.firePoints.Length;
        return point;
    }
}
