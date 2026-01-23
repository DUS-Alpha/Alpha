using UnityEngine;
using UnityEngine.UIElements;

namespace alpha
{
    // 탄환형
    public enum EMainRangeTypes
    {
        Rifle,
        Sniper,
        Machinegun
    }
    [CreateAssetMenu(fileName = "MainRangeWeapon", menuName = "Scriptable Objects/Item/MainRangeWeapon")]
    // 현재는 히트스캔방식으로 공격 처리하기(현실감x, 빠른템포의 게임은 이방식으로)
    public class MainRangeWeaponSO : WeaponSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            WeaponType = EWeaponTypes.MainRange;
        }
#endif

        [Space(10)]
        [Header("[ Range ]")]
        public EMainRangeTypes RangeType;
        public float Distance;
        public float FireRate;
    }
}