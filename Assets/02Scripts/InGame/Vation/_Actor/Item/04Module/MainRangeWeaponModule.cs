using UnityEngine;

namespace alpha
{
    public class MainRangeWeaponModule : ItemModuleBase
    {
        public MainRangeWeaponSO MainRangeWeaponData => Data as MainRangeWeaponSO;

        public Transform LeftHandTr;
        public Transform RightHandTr;
    }
}