using UnityEngine;

namespace alpha
{
    public enum  ESpeedTypes
    {
        Base,
        Back,
        Jump,
        CombatReady,
        CombatBack,
        Dash,
        FlightVertical,
        FlightMove
    }

    public class LocomotionRules
    {
        public bool Grounded ()
        {
            return true;
        }
        public bool CanMove(bool isGrounded)
        {
            return isGrounded;
        }

        public bool CanJump(bool isGrounded)
        {
            return isGrounded;
        }

        public bool CanDash(float gauge)
        {
            if(gauge < 0.4f) return false;
            return true;
        }

        public bool CanFall(bool isGrounded)
        {
            return !isGrounded;
        }

        public bool CanFlyUp(float gauge)
        {
            return gauge > 0.1f;
        }

        public bool CanFlyDown()
        {
            return true;
        }

        public bool CanFlyMove()
        {
            return true;
        }
    }
}