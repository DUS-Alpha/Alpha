using System;
using UnityEngine;

public class BreathCycle : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public NodeState CheckBreath(BreathSetting breathSetting)
    {
        float elapsed = Time.time - breathSetting.lastBreathTime;
        bool canUse = elapsed >= breathSetting.breathCooldown;

        /*Debug.Log($"[CheckBreath] elapsed={elapsed:F2} / cooldown={breathCooldown} / useBreath={useBreath}");*/

        if (!breathSetting.useBreath && canUse)
        {
            breathSetting.useBreath = true;
            //Debug.Log("[CheckBreath] ✅ 브레스 발동 준비 완료");
            return NodeState.Success;
        }

        //Debug.Log("[CheckBreath] ❌ 실패 (쿨타임 중이거나 이미 발동됨)");
        return NodeState.Failure;
    }
    
    public NodeState BreatheFire(bool isRunning ,bool isComplete)
    {
        if (!isRunning)
        {
            animator.SetTrigger("Breath");
            isRunning = true;
            isComplete = false;
        }

        if (!isComplete)
            return NodeState.Running;

        // 애니메이션 종료 후
        isRunning = false;
        return NodeState.Success;
        
    }
    
    public void SlowSpeed()
    {
        animator.speed = 0.5f;
        print($"{animator.speed}");
    }
    public void NormalSpeed()
    {
        animator.speed = 1f;
        print($"{animator.speed}");
    }

    public void BreathprefabsActiveTrue()
    {
        var state = GetComponent<DragonBossActions>();
        state.currentBreathsetting.breathPrefab.SetActive(true);
    }
    
    public void BreathprefabsActiveFalse()
    {
        var state = GetComponent<DragonBossActions>();
        state.currentBreathsetting.breathPrefab.SetActive(false);
    }

}
