using UnityEngine;

public enum RangeTypes
{
    Pistol,
    Rifle,
    Sniper,
    Machinegun
}

// 현재는 히트스캔방식으로 공격 처리하기(현실감x, 빠른템포의 게임은 이방식으로)
public class RangeWeapon : Weapon
{
    public RangeTypes RangeType;

    [SerializeField]
    private float m_maxDistance = 20f;
    [SerializeField]
    private int m_maxAmmo;
    [SerializeField]
    private int m_ammo;

    public AudioSource m_audioSource;
    [SerializeField]
    private Transform m_bulletFirePos; //이펙트 효과만
    [SerializeField]
    private ParticleSystem m_muzzleFlashEffect;
    


    private bool m_isFire;
    
    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        m_isFire = isAttackInput;

        if (!isAttackInput) return;
        anim.RangeShootingAni();
        m_audioSource.PlayOneShot(m_audioSource.clip);
        m_muzzleFlashEffect.Play();

        ApplyDamageInfo();
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isFire;
    }
    public void Reload()
    {

    }
    public void ApplyDamageInfo()
    {
        Vector3 _rayOrigin = Camera.main.transform.position;
        Vector3 _rayDirection = Camera.main.transform.forward; // 화면 중앙 기준
        RaycastHit _hit;

        if (Physics.Raycast(_rayOrigin, _rayDirection, out _hit, m_maxDistance))
        {
            // 데미지 받을 타겟
            IDamageable _damageableTarget;
            if (_hit.collider.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                // 데미지 정보
                DamageMassage _damageMassage = new DamageMassage();
                //_damageMassage.Damager = damager;
                _damageMassage.HitNormal = _hit.normal;
                _damageMassage.HitPoint = _hit.point;
                _damageMassage.damage = this.WeaponData.AttackDamage;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);

                // 맞은 곳에 BloodEffect 재생(몬스터쪽에서해도 괜찮음)
            }
        }
    }
}
