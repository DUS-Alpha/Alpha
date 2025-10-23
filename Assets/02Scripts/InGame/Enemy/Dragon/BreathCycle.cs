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
    
    public NodeState BreatheFire(Animator animator, BreathSetting breathSetting)
    {
        breathSetting.useBreath = true;
        
        if (!breathSetting.useBreath) 
        {
            //Debug.Log("[BreatheFire] ❌ 발동 조건 미충족 (useBreath == false)");
            return NodeState.Failure;
        }

        if (!breathSetting._breathStarted)
        {
            //Debug.Log("[BreatheFire] ▶ FireBreath 애니메이션 트리거 발동");
            animator.SetTrigger("Breath");
            breathSetting._breathStarted = true;
            return NodeState.Running;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);
        // Debug.Log($"[BreatheFire] 현재 상태: {state.fullPathHash}, normalizedTime={state.normalizedTime:F2}");
        
        if (state.IsName("Breath"))
        {
            if (state.normalizedTime >= 0.95f)
            {
                // Debug.Log("[BreatheFire] ✅ 브레스 완료");
                breathSetting._breathStarted = false;
                breathSetting.useBreath = false;
                breathSetting.lastBreathTime = Time.time;
                return NodeState.Success;
            }
            else
            {
                // Debug.Log("[BreatheFire] ⏳ 브레스 애니메이션 진행 중");
            }
        }
        else
        {
            // Debug.LogWarning("[BreatheFire] ⚠ FireBreath 상태가 아님 — Animator 상태 이름 확인 필요");
        }

        return NodeState.Running;
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
