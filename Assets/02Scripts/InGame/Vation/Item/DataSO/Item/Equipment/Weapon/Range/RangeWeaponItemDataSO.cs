using UnityEngine;
using UnityEngine.UIElements;

namespace alpha
{
    public enum ERangeItemTypes
    {
        Rifle,
        Sniper,
        Machinegun
    }
    [CreateAssetMenu(fileName = "RangeWeapon", menuName = "Scriptable Objects/ItemData/RangeWeapon")]
    // 현재는 히트스캔방식으로 공격 처리하기(현실감x, 빠른템포의 게임은 이방식으로)
    public class RangeWeaponItemDataSO : WeaponItemDataSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }
#endif

        [Space(10)]
        [Header("[ Range ]")]
        public ERangeItemTypes RangeType;
        public float Distance;
        public float FireRate;
    }
}