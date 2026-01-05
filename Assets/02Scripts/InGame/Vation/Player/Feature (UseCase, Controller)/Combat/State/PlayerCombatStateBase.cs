using UnityEngine;

namespace alpha
{
    public abstract class PlayerCombatStateBase : PlayerStateBase
    {
        public PlayerCombatStateBase(PlayerCore playerCore) : base(playerCore) { }

        public abstract override void Enter();

        public abstract override void Update();

        public abstract override void Exit();
    }
}