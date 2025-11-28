using alpha;
using UnityEngine;

namespace alpha
{
    public class MeleeWeaponItem : WeaponItem
    {
        public MeleeWeaponItemDataSO MeleeData => (MeleeWeaponItemDataSO)Data;

        public bool CanMoveDuringAttack => false;

        protected override void Awake()
        {
            AttackStrategy = new MeleeAttackStrategy();
        }

        //public override void Attack(){}
    }
}