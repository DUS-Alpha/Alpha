using System;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool m_isCombo;
    [SerializeField]
    private Collider m_collider;
    
    private int m_index = 0;

    private void Start()
    {
        SetActivateCollider(false);
    }

    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        
    }
    public override void Unequip(GameObject user)
    {
        m_isCombo = false;
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isCombo;
    }
    public void SetActivateCollider(bool isActivate)
    {
        m_collider.enabled = isActivate;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IDamageable _damageableTarget;
            if (other.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.damage = this.WeaponData.AttackDamage;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);
                // 맞은 곳에 BloodEffect 재생(몬스터쪽에서해도 괜찮음)

                Debug.Log(other.name);
            }
        }
    }
    public void DoDamage(Transform pos,float radius)
    {
        Collider[] hits = Physics.OverlapSphere(pos.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out IDamageable _damageableTarget))
            {
                DamageMassage _damassage = new DamageMassage
                {
                    //HitNormal = hit.normal,
                    //HitPoint = hit.point,
                    damage = WeaponData.AttackDamage
                };
                _damageableTarget.ApplyDamage(_damassage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
       // Gizmos.DrawWireSphere(transform.position, radius);
    }
}
