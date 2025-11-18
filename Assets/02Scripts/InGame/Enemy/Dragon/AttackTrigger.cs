using System;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]private float Damage = 10; //데미지 테스트용
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("플레이어 접촉");
            
            if (other.TryGetComponent<IDamageable>(out IDamageable _damageableTarget))
            {
                DamageMassage _damassage = new DamageMassage
                {
                    damage = Damage
                };
                print($"데미지 피격 : {Damage}");
                _damageableTarget.ApplyDamage(_damassage);
            }
        }
    }
    
}
