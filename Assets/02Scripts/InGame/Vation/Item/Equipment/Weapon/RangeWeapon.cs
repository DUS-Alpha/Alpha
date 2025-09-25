using UnityEngine;
using UnityEngine.UIElements;

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
    [Space(10)]

    [Header("[ Range ]")]
    public RangeTypes RangeType;
    [SerializeField]
    private float m_maxDistance = 40f;
    [SerializeField]
    private int m_maxAmmo;
    [SerializeField]
    private int m_currentAmmo;
    [SerializeField]
    private int m_saveAmmo;
    public int MaxAmmo => m_maxAmmo;
    public int CurrentAmmo => m_currentAmmo;
    public int SaveAmmo => m_saveAmmo;
    
    public AudioSource m_audioSource;
    [SerializeField]
    private Transform m_bulletFirePos; //이펙트 효과만
    [SerializeField]
    private ParticleSystem m_muzzleFlashEffect;
    
    private bool m_isFire;
    public bool IsDistance => m_isDistance;
    private bool m_isDistance;
    private bool m_canNeedReload;
    public bool IsNeedReload => m_canNeedReload;

    private void Start()
    {
        m_currentAmmo = m_maxAmmo;
    }

    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        if (m_currentAmmo == 0)
        {
            // 빈 발사 소리
            return;
        }
        m_isFire = isAttackInput;

        if (!m_isFire) return;
        anim.AttackAni(m_isFire, 2);
        m_audioSource.PlayOneShot(m_audioSource.clip);
        m_muzzleFlashEffect.Play();

        if(m_currentAmmo > 0)
            m_currentAmmo -= 1;

        ApplyDamageInfo();
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isFire;
    }
    public bool Reload()
    {
        int _needAmmo = m_maxAmmo - m_currentAmmo;

        if (m_saveAmmo == 0 || _needAmmo == 0) return false;

        if(_needAmmo <= m_saveAmmo)
        {
            m_currentAmmo = m_maxAmmo;
            m_saveAmmo -= _needAmmo;
        }
        else if(_needAmmo > m_saveAmmo)
        {
            m_currentAmmo += m_saveAmmo;
            m_saveAmmo = 0;
        }
        return true;
    }
    public void ApplyDamageInfo()
    {
        Vector3 _rayOrigin = Camera.main.transform.position;
        Vector3 _rayDirection = Camera.main.transform.forward; // 화면 중앙 기준
        RaycastHit _hit;

        if (Physics.Raycast(_rayOrigin, _rayDirection, out _hit, m_maxDistance))
        {
            Debug.DrawLine(_rayOrigin, _hit.point, Color.red);
            // 데미지 받을 타겟
            IDamageable _damageableTarget;
            if (_hit.collider.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                m_isDistance = true;
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
        else m_isDistance = false;
    }
}
