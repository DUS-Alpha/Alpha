using UnityEngine;

namespace alpha
{
    // 이동에 대한 규칙
    public class LocomotionRules
    {
        public bool CanMove()
        {
            // == GroundMove ==
            // !Jump, !FlyUp, !Fall, !Dash, !Melee Attack, !SkillAttack

            // == FlyMove ==
            // !Jump, !FlyUp, !Fall, !Dash, !SkillAttack

            return true;
        }

        public bool CanRotate()
        {
            // !LockRotate, !Jump, !FlyUp, !Dash, !Melee Attack, !SkillAttack
            return true;
        }

        public bool CanJump()
        {
            // !FlyUp, !FlyMove, !Fall, !Dash, !Melee Attack, !SkillAttack
            return true;
        }

        public bool CanFlyUp()
        {
            // SuitGauge > 0, !FlyMove, !Jump, !Dash, !Melee Attack, !SkillAttack
            return true;
        }

        // Fly중 Fall로 만드는 기능
        // Fly중인 상태를 전재로
        public bool CanFlyDown()
        {
            // SuitGauge <= 0(자동), !SkillAttack, !Dash
            return true;
        }

        public bool CanDash()
        {
            // SuitGauge > 0, !Jump, !Melee Attack, !SkillAttack
            return true;
        }


    }
}