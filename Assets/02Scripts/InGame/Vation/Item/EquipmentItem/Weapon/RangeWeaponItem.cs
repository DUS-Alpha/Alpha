using UnityEngine;

namespace alpha
{
    public class RangeWeaponItem : WeaponItem
    {
        public RangeWeaponItemDataSO RangeData => (RangeWeaponItemDataSO)Data;
        public override void Attack()
        {
            
        }

        /*private void Awake()
{
    m_currentAmmo = m_maxAmmo;
    m_audioSource.clip = m_audioClip;
}

*//*public override void Attack(bool isAttackInput, PlayerAnimationController anim)
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

    if (m_currentAmmo > 0)
        m_currentAmmo -= 1;
}*//*

public bool Reload()
{
    int _needAmmo = m_maxAmmo - m_currentAmmo;

    if (m_saveAmmo == 0 || _needAmmo == 0) return false;

    if (_needAmmo <= m_saveAmmo)
    {
        m_currentAmmo = m_maxAmmo;
        m_saveAmmo -= _needAmmo;
    }
    else if (_needAmmo > m_saveAmmo)
    {
        m_currentAmmo += m_saveAmmo;
        m_saveAmmo = 0;
    }
    return true;
}*/
    }
}