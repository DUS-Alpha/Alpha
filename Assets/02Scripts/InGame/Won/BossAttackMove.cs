using UnityEngine;
using System.Collections;

public class BossAttackMove : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 3f;
    public float moveDuration = 1.0f; // 이동 지속 시간

    bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            // 애니메이션 중 이동
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    public void StartAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        animator.SetTrigger("Attack"); // 애니메이션 재생
        yield return new WaitForSeconds(0.3f); // 선딜 (모션이 뜨는 시점)
        
        isMoving = true;
        yield return new WaitForSeconds(moveDuration); // 이동 지속
        isMoving = false;

        // 후딜 동안 대기
        yield return new WaitForSeconds(0.5f);
    }
}