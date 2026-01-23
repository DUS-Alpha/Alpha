using UnityEngine;

namespace alpha
{
    public interface ILocomotion
    {
        void ExcuteMove(Vector2 moveInput, float speed, CombatConstraint combatConstraint);
    }
}