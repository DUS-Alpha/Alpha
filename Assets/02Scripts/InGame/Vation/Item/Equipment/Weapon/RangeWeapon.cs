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
    private int m_maxAmmo;
    [SerializeField]
    private int m_currentAmmo;
    [SerializeField]
    private int m_saveAmmo;

    [SerializeField]
    private LayerMask m_layer;
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
        CameraShakeManager.Instance.Shake(ShakeType.Shooting);

        if(m_currentAmmo > 0)
            m_currentAmmo -= 1;
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
}
