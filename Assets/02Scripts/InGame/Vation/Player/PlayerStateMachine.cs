using System;
using System.Collections.Generic;
using UnityEngine;

// Locomotion FSM, Combat FSM 병렬 처리
// 상태에 따른 각 FSM들에게 입력 IsInputLocked 서로 적용 가능
// 각 FSM내부에서 Flags 관리
// 병렬 처리 이유 : 응집도↑, 테스트 쉬움, 상체 공격 + 하체 이동 같은 병행 동작 가능
public class PlayerStateMachine
{
    private PlayerCore m_playerCore;
    // Locomotion
    public LocomotionStateType CurrentLocomotion => m_currentLocoType;
    private LocomotionStateType m_currentLocoType;
    private PlayerState m_locoState;

    // Combat
    public CombatStateType CurrentCombat => m_currentCombatType;
    private CombatStateType m_currentCombatType;
    public CombatStateType m_prevCombatType { get; private set; }
    public CombatStateType PrevCombatType => m_prevCombatType;
    private PlayerState m_combatState;

    // 딕셔너리 초기화시 value값에 new 생성자를 하면 Key에 대한 Value는 이미 new로 처음 생성된 인스턴스를 재사용한것.
    // Func타입으로 함수로 new 생성자 처리 시 새 인스턴스
    private Dictionary<LocomotionStateType, Func<PlayerState>> m_locomotionStateCreateDic;
    private Dictionary<CombatStateType, Func<PlayerState>> m_combatStateCreateDic;

    public void InitializeMoudle(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
        m_locomotionStateCreateDic = new Dictionary<LocomotionStateType, Func<PlayerState>>
        { 
            // 일반 new로 작성시 상태가 현재 Dic 선언시에 객체로 저장이 되어져 그저 해당 상태를 재사용하는꼴임
            // Func을 통한 함수로써 객체를 만든다의 방식은 완전한 새로운 객체를 생성해내는 것
            {LocomotionStateType.Idle, ()=> new PlayerIdleState(m_playerCore) },
            {LocomotionStateType.Move, ()=> new PlayerMoveState(m_playerCore) },
            {LocomotionStateType.Jump, () => new PlayerJumpState(m_playerCore) },
            {LocomotionStateType.Landing, () => new PlayerLandingState(m_playerCore) },
            {LocomotionStateType.Dodge, () => new PlayerDodgeState(m_playerCore) },
            {LocomotionStateType.Fall, () => new PlayerFallState(m_playerCore) },
            {LocomotionStateType.FlyUp, () => new PlayerFlyUpState(m_playerCore) },
            {LocomotionStateType.Flying, () => new PlayerFlyingState(m_playerCore) },
        };

        m_combatStateCreateDic = new Dictionary<CombatStateType, Func<PlayerState>>
        {
            { CombatStateType.Idle, ()=>  new PlayerCombatIdleState(m_playerCore) },
            { CombatStateType.Aim, ()=>  new PlayerAmingState(m_playerCore) },
            {CombatStateType.SwapWeapon, ()=> new PlayerSwapWeaponState(m_playerCore) },
            { CombatStateType.Attack, ()=>  new PlayerAttackState(m_playerCore) },
            { CombatStateType.Reload, ()=>  new PlayerReloadState(m_playerCore) }
        };
    }

    public void Update()
    {
        m_locoState.Update();

        m_combatState.Update();
    }

    public void SwitchLocomotionState(LocomotionStateType newState,InputCombatLockType inputCombatLockType = InputCombatLockType.None)
    {
        if (newState == m_currentLocoType) return;
        Func<PlayerState> _newState = m_locomotionStateCreateDic[newState];
        m_locoState?.Exit();

        m_locoState = _newState();
        m_currentLocoType = newState;

        m_locoState.Enter();
    }

    public void SwitchCombatState(CombatStateType newState)
    {
        if (newState == m_currentCombatType) return;
        Func<PlayerState> _newState = m_combatStateCreateDic[newState];
        m_combatState?.Exit();

        m_prevCombatType = CurrentCombat;
        m_combatState = _newState();
        m_currentCombatType = newState;

        m_combatState.Enter();
    }
}
