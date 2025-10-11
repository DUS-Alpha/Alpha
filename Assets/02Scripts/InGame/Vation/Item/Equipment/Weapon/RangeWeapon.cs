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
    private AudioClip m_audioClip;
    [SerializeField]
    private Transform m_bulletFirePos; //이펙트 효과만
    [SerializeField]
    private ParticleSystem m_muzzleFlashEffect;
    
    private bool m_isFire;
    private bool m_canNeedReload;
    public bool IsNeedReload => m_canNeedReload;

    private void Awake()
    {
        m_currentAmmo = m_maxAmmo;
        m_audioSource.clip = m_audioClip;
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
        anim.RangeShootingAni();
        m_audioSource.PlayOneShot(m_audioSource.clip);
        m_muzzleFlashEffect.Play();

        if(m_currentAmmo > 0)
            m_currentAmmo -= 1;

        if (TryGetTarget(out RaycastHit hit))
            ApplyDamage(hit);
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
    /// <summary>
    /// 거리 내 맞은 대상이 있는지 확인 (단순 체크용)
    /// </summary>
    public bool TryGetTarget(out RaycastHit hit)
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;

        bool isHit = Physics.Raycast(origin, dir, out hit, m_maxDistance);

        if (isHit)
        {
            Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
        }

        return isHit;
    }

    /// <summary>
    /// 실제 데미지를 적용하는 메서드 (거리 체크 통과 후 호출)
    /// </summary>
    private void ApplyDamage(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
        {
            DamageMassage msg = new DamageMassage
            {
                HitNormal = hit.normal,
                HitPoint = hit.point,
                damage = WeaponData.AttackDamage
            };

            target.ApplyDamage(msg);
            // TODO: 피격 이펙트 재생 등
        }
    }
}
