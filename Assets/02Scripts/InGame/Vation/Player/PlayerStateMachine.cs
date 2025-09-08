using System;
using System.Collections.Generic;
using UnityEngine;

public enum LocomotionState
{
    None,
    Idle,
    Move,
    Jump,
    Fall,
    FlyStartUp,
    Flying
}

public enum CombatState
{
    None,
    CombatIdle,
    Aim,
    Attack,
    Swap,
    Reload
}

// 하나의 상태머신에서 Locomotion, Combat 병렬로 실행
// 애니메이션 Layer관리와 함께 구조적 관리가 수월
public class PlayerStateMachine : MonoBehaviour
{
    // Locomotion
    private PlayerCore m_playerCore;
    private PlayerState m_currentLocoState;
    private Dictionary<LocomotionState, Func<PlayerState>> m_locomotionStateCreateDic;  // 일반 new로 작성시 재사용으로되는거지만 이렇게작성 시 새 상태를 받게되는것
    public LocomotionState CurrentLocoState { get; private set; }
    //public LocomotionState PrevLocoState { get; private set; }    // TODO : 차후에 필요할 시 적용

    // Combat
    private PlayerState m_currentCombatState;
    private Dictionary<CombatState, Func<PlayerState>> m_combatStateCreateDic;
    public CombatState CurrentCombatState { get; private set; }
    //public CombatState PrevCombatState { get; private set; }

    private void Awake()
    {

    }
    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;

        m_locomotionStateCreateDic = new Dictionary<LocomotionState, Func<PlayerState>>
        { 
            // 일반 new로 작성시 상태가 현재 Dic 선언시에 객체로 저장이 되어져 그저 해당 상태를 재사용하는꼴임
            // Func을 통한 함수로써 객체를 만든다의 방식은 완전한 새로운 객체를 생성해내는 것
            {LocomotionState.Idle, ()=> new PlayerIdleState(m_playerCore) }, 
            {LocomotionState.Move, ()=> new PlayerMoveState(m_playerCore) },
            {LocomotionState.Jump, () => new PlayerJumpState(m_playerCore) },
            {LocomotionState.Fall, () => new PlayerFallState(m_playerCore) },
            {LocomotionState.FlyStartUp, () => new PlayerFlyUpStartState(m_playerCore) },
            {LocomotionState.Flying, () => new PlayerFlyingState(m_playerCore) },
        };

        m_combatStateCreateDic = new Dictionary<CombatState, Func<PlayerState>>
        {
            {CombatState.CombatIdle, () => new PlayerCombatIdleState(m_playerCore)},
            {CombatState.Aim, () => new PlayerAimState(m_playerCore)},
            {CombatState.Reload, () => new PlayerReloadState(m_playerCore)},
            {CombatState.Swap, () => new PlayerSwapWeaponState(m_playerCore)}
        };
    }

    private void Start()
    {
        SwitchLocomotionState(LocomotionState.Idle);
        SwitchCombatState(CombatState.CombatIdle);
    }

    private void Update()
    {
        m_currentLocoState.Update();
        m_currentCombatState?.Update();
    }

    public void SwitchLocomotionState(LocomotionState newLocoState)
    {
        if (newLocoState == CurrentLocoState) return;
        Func<PlayerState> _newState = m_locomotionStateCreateDic[newLocoState];
        m_currentLocoState?.Exit();

        m_currentLocoState = _newState();
        CurrentLocoState = newLocoState;

        m_currentLocoState.Enter();
    }

    public void SwitchCombatState(CombatState newCombatState)
    {
        if (newCombatState == CurrentCombatState) return;
        Func<PlayerState> _newState = m_combatStateCreateDic[newCombatState];
        m_currentCombatState?.Exit();

        m_currentCombatState = _newState();
        CurrentCombatState = newCombatState;

        m_currentCombatState.Enter();
    }

}
