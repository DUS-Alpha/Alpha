using UnityEngine;

namespace alpha
{
    public class SubRangeWeaponModule : ItemModuleBase
    {
        public MainRangeWeaponSO SubRangeWeaponData => Data as MainRangeWeaponSO;

        public Transform LeftHandTr;
        public Transform RightHandTr;
    }
}
