using UnityEngine;

namespace alpha
{
    public interface IActionPolicy
    {
        bool CanMove(bool isGrounded, CombatStateData combatData);
        bool CanJump(bool isGrounded, CombatStateData combatData);
        bool CanDash(float gauge, CombatStateData combatData);
        bool CanFlyUp(CombatStateData combatData);
    }
}