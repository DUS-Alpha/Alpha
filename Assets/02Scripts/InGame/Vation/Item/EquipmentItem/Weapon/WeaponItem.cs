using UnityEngine;

namespace alpha
{
    public abstract class WeaponItem : Item
    {
        public WeaponItemDataSO WeaponData => (WeaponItemDataSO)Data;

        public Transform LeftHandTr;
        public Transform RightHandTr;
        /// <summary>
        /// 이펙트 나오는 위치
        /// </summary>
        public Transform EffectTr;

        public IAttackStrategy AttackStrategy { get; protected set; }

        protected abstract void Awake();
        //public abstract void Attack();
    }
}