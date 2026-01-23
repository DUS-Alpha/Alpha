using UnityEngine;

namespace alpha
{
    public class MeleeWeaponModule : ItemModuleBase
    {
        public MeleeWeaponSO MeleeWeaponData => Data as MeleeWeaponSO;

        public Transform LeftHandTr;
        public Transform RightHandTr;
    }
}
