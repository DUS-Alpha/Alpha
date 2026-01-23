using UnityEngine;

namespace alpha
{
    public class RangeWeaponItem : WeaponItem
    {
        public MainRangeWeaponSO RangeData => (MainRangeWeaponSO)Data;

        [SerializeField] private ParticleSystem m_muzzleFlashEffect;
        protected override void Awake()
        {
            AttackStrategy = new RangeWeaponStrategy();
        }
        public void PlayEffect()
        {
            m_muzzleFlashEffect.Play();
        }
    }
}