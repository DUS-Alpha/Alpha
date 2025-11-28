using UnityEngine;

namespace alpha
{
    public class RangeWeaponItem : WeaponItem
    {
        public RangeWeaponItemDataSO RangeData => (RangeWeaponItemDataSO)Data;

        protected override void Awake()
        {
            AttackStrategy = new RangeAttackStrategy();
        }
        //public override void Attack(){}
    }
}