using System;
using UnityEngine;

public class BreathCycle : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
