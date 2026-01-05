using Unity.VisualScripting;
using UnityEngine;

namespace alpha
{
    // 상태 제약
    public class PlayerStateContext
    {
        // Locomotion
        public bool IsGrounded { get; set; }
        public bool IsFly { get; set; }
        public bool IsVerticalTakeOff { get; set; }
        public bool IsMove { get; set; }
        public bool IsJump { get; set; }
        public bool IsFall { get; set; }
        public bool IsLand { get; set; }
        public bool IsDie { get; set; }


        // Combat
        public bool IsAttack { get; set; }
        public bool IsSwap { get; set; }
        public bool IsSkill { get; set; }
        public bool IsCombatReady { get; set; }


        public EWeaponTypes CurrentWeaponType { get; set; }
        // Melee or Range

        public bool IsMelee => CurrentWeaponType == EWeaponTypes.Melee;
        public bool IsRange => (CurrentWeaponType == EWeaponTypes.MainRange) || (CurrentWeaponType == EWeaponTypes.SubRange);

        // TODO : Interaction


        #region ======================================== CAN
        // Locomotion과 Combat간 서로의 상태들에 따른 Can
        public bool CanMoveByCombat
        {
            get
            {
                if (IsSkill) return false;
                if (IsAttack)
                {
                    if (IsMelee)
                        return false;  // 근접 → 이동 금지
                    if (IsRange)
                        return true;   // 원거리 → 이동 허용
                }

                // 기본은 이동 가능
                return true;
            }
        }

        public bool CanRotByCombat
        {
            get
            {
                if (IsSkill) return false;

                if (IsAttack)
                {
                    if (IsMelee)
                        return false;  // 근접 → 이동 금지
                    if (IsRange)
                        return true;   // 원거리 → 이동 허용
                }
                return true;
            }
        }

        public bool CanJumpByCombat
        {
            get
            {
                if (IsSkill) return false;
                return true;
            }
        }
        public bool CanVerticalTakeOffByCombat
        {
            get
            {
                if (IsSkill) return false;

                if (IsAttack)
                {
                    if (IsMelee)
                        return false;  // 근접 → 이동 금지
                    if (IsRange)
                        return true;   // 원거리 → 이동 허용
                }
                return true;
            }
        }
        
        public bool CanAttackByLoco
        {
            get
            {
                if (IsJump) return false;
                if(IsFall) return false;
                if(IsVerticalTakeOff) return false;
                if (IsLand) return false;
                if(IsDie) return false;
                return true;
            }
        }
        public bool CanSwapByLoco
        {
            get
            {
                if (IsJump) return false;
                if (IsFall) return false;
                if (IsVerticalTakeOff) return false;
                if (IsLand) return false;
                if (IsDie) return false;
                return true;
            }
        }
        public bool CanSkillByLoco
        {
            get
            {
                if (IsJump) return false;
                if (IsFall) return false;
                if (IsVerticalTakeOff) return false;
                if (IsLand) return false;
                if (IsDie) return false;
                return true;
            }
        }
        #endregion ======================================== /CAN
    }
}