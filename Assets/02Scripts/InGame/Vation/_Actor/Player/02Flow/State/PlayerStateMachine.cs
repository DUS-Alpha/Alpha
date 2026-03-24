using System;
using System.Collections.Generic;
using UnityEngine;

// 상태 흐름 관리 클래스
namespace alpha
{
    public enum LocomotionStateType
    {
        None,
        Idle,
        Move,
        Jump,
        Fall,
        Land,
        Dash,
        FlyUp,
        FlightMove,
        Die
    }
    public enum CombatStateType
    {
        None,
        NonCombat,
        Swap,
        Attack,
        CombatReady,
        Skill,
    }
    // Locomotion FSM, Combat FSM 병렬 처리(패럴상태머신)
    // 병렬 처리 이유 : 상체 공격 + 하체 이동 같은 병행 동작
    public class PlayerStateMachine : MonoBehaviour
    {
        // Locomotion
        private PlayerStateBase m_locoState;
        private Dictionary<LocomotionStateType, Func<PlayerStateBase>> m_locomotionStateCreateDic;
        public LocomotionStateType CurrentLocoState { get; private set; }

        // Combat
        private PlayerStateBase m_combatState;
        private Dictionary<CombatStateType, Func<PlayerStateBase>> m_combatStateCreateDic;
        public CombatStateType CurrentCombatState{ get; private set; }

        public void OnStart(PlayerCore core)
        {
            m_locomotionStateCreateDic = new Dictionary<LocomotionStateType, Func<PlayerStateBase>>
            { 
                // 일반 new로 작성시 상태가 현재 Dic 선언시에 객체로 저장이 되어져 그저 해당 상태를 재사용하는꼴임
                // Func을 통한 함수로써 객체를 만든다의 방식은 완전한 새로운 객체를 생성해내는 것
                {LocomotionStateType.Idle, ()=> new PlayerIdleState(core) },
                {LocomotionStateType.Move, ()=> new PlayerMoveState(core) },
                {LocomotionStateType.Jump, () => new PlayerJumpState(core) },
                {LocomotionStateType.Land, () => new PlayerLandState(core) },
                {LocomotionStateType.Fall, () => new PlayerFallState(core) },
                {LocomotionStateType.Dash, () => new PlayerDashState(core) },
                {LocomotionStateType.FlyUp, () => new PlayerFlyUpState(core) },
                {LocomotionStateType.FlightMove, () => new PlayerFlightMoveState(core) },
                {LocomotionStateType.Die, () => new PlayerDieState(core) }
            };
            

            m_combatStateCreateDic = new Dictionary<CombatStateType, Func<PlayerStateBase>>
            {
                {CombatStateType.NonCombat, ()=>  new PlayerNonCombatState(core) },
                {CombatStateType.Swap, ()=> new PlayerSwapState(core) },
                {CombatStateType.Attack, ()=> new PlayerAttackState(core) },
                {CombatStateType.CombatReady, ()=> new PlayerCombatReadyState(core) },
                {CombatStateType.Skill, ()=>  new PlayerSkillState(core) },
            };

            m_locoState = m_locomotionStateCreateDic[LocomotionStateType.Idle]();
            m_combatState = m_combatStateCreateDic[CombatStateType.NonCombat]();
        }

        public void OnUpdate()
        {
            m_locoState.Update();
            m_combatState.Update();
        }

        // 상태 변경 관리
        public void SwitchLocomotionState(LocomotionStateType newState)
        {
            if (newState == CurrentLocoState) return;
            Func<PlayerStateBase> _newState = m_locomotionStateCreateDic[newState];
            m_locoState?.Exit();

            m_locoState = _newState();
            CurrentLocoState = newState;

            m_locoState.Enter();
        }

        public void SwitchCombatState(CombatStateType newState)
        {
            if (newState == CurrentCombatState) return;
            Func<PlayerStateBase> _newState = m_combatStateCreateDic[newState];
            m_combatState?.Exit();

            m_combatState = _newState();
            CurrentCombatState = newState;

            m_combatState.Enter();
        }

    }
}