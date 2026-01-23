using System;
using UnityEngine;
namespace alpha
{
    [CreateAssetMenu(fileName = "MeleeWeapon", menuName = "Scriptable Objects/Item/MeleeWeapon")]
    public class MeleeWeaponSO : WeaponSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            WeaponType = EWeaponTypes.Melee;
        }

#endif
    }
}