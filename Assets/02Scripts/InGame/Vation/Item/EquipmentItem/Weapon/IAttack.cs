using UnityEngine;

namespace alpha
{
    public interface IAttack
    {
        void Attack(bool isAttackInput, PlayerAnimationController anim);
    }
}