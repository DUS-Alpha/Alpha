using UnityEngine;

namespace alpha
{
    public class RangeWeaponItem : WeaponItem
    {
        public RangeWeaponItemDataSO RangeData => (RangeWeaponItemDataSO)Data;

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