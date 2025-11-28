using UnityEngine;

namespace alpha
{
    public interface IAttackStrategy
    {
        /// <summary> 공격이 시작될 때(Enter Attack State) </summary>
        void StartAttack(PlayerCombat combat);

        void UpdateAttack(PlayerCombat combat);

        /// <summary> 공격 종료(Exit Attack State) </summary>
        void ExitAttack(PlayerCombat combat);

        /// <summary> 공격 중 이동 가능한가? </summary>
        bool CanMoveDuringAttack { get; }

    }
}