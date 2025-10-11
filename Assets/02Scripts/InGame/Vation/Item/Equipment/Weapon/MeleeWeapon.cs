using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool m_isCombo;
    [SerializeField]
    private Collider m_collider;
    private void Start()
    {
        SetActivateCollider(false);
    }

    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        anim.MeleeComboTriggerAni();
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
}
