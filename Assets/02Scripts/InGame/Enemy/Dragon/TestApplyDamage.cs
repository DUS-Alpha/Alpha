using System;
using UnityEngine;
using UnityEngine.UI; // 버튼 쓰려면 필요

public class TestApplyDamage : MonoBehaviour
{
    //플레이어
    public GameObject target;
    //보스 
    public GameObject Boss;
    [SerializeField] private Button attackButton;      // UI 버튼
    [SerializeField] private Button BreathsButton;      // UI 버튼

    private void Start()
    {
        if (attackButton != null)
        {
            // 버튼 클릭 시 Attack 함수 실행
            attackButton.onClick.AddListener(attack);
        }
        if (BreathsButton != null)
        {
            // 버튼 클릭 시 Attack 함수 실행
            BreathsButton.onClick.AddListener(Breath);
        }
    }


    public void Breath()
    {
        DragonBossActions dragon = Boss.GetComponent<DragonBossActions>();
        dragon.useBreath = true;
    }

    public void attack()
    {
        DragonBossActions dragon = Boss.GetComponent<DragonBossActions>();
        var damagemessage = new DamageMassage()
        {
            Damager =  target,
            damage = target.GetComponent<TestPlayerController>().damage
        };
        
        dragon.ApplyDamage(damagemessage);
    }
}

