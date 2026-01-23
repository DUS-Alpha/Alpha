using UnityEngine;

namespace alpha
{
    public abstract class WeaponItem : Item
    {
        public WeaponSO WeaponData => (WeaponSO)Data;

        public Transform LeftHandTr;
        public Transform RightHandTr;

        public IAttackStrategy AttackStrategy { get; protected set; }

        protected abstract void Awake();

    }
}