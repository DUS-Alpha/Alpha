using UnityEngine;
using UnityEngine.UIElements;

namespace alpha
{
    // 범위형 무기
    public enum ESubRangeTypes
    {
        FireGun,
    }
    [CreateAssetMenu(fileName = "SubRangeWeapon", menuName = "Scriptable Objects/Item/SubRangeWeapon")]
    // 현재는 히트스캔방식으로 공격 처리하기(현실감x, 빠른템포의 게임은 이방식으로)
    public class SubRangeWeaponSO : WeaponSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            WeaponType = EWeaponTypes.SubRange;
        }
#endif

        [Space(10)]
        [Header("[ Sub Range ]")]
        public ESubRangeTypes RangeType;
        public float Distance;
        public float FireRate;
    }
}