using alpha;
using UnityEngine;

namespace alpha
{
    public class MeleeWeaponItem : WeaponItem
    {
        public MeleeWeaponSO MeleeData => (MeleeWeaponSO)Data;

        public bool CanMoveDuringAttack => false;

        protected override void Awake()
        {
            AttackStrategy = new MeleeWeaponStrategy();
        }

        //public override void Attack(){}
    }
}