using System;
using System.Collections.Generic;
using UnityEngine;

// 평소: LocomotionState가 Base인 상태에서 + CombatFlagsState 처리
// 특정 상황(스킬, 공격, 회피): CombatFullState 활성화 → LocomotionState를 잠시 무시하고 Combat 전권

// Locomotion, CombatFullState, CombatFlags의 Lock/Unlock을 단순 Boolean으로 관리
// 입력 처리 시 CanMove / CanUseCombat 확인
public class PlayerStateMachine : MonoBehaviour
{
    private PlayerCore m_playerCore;

    private PlayerState m_locoState;
    private PlayerState m_combatFullState;

    private CombatFlagsController m_combatFlagsController;

    // TODO : 상호작용이나 체력관련한 Flags도 추가 예정

    private LocomotionStateType m_locoType;
    private CombatFullStateType m_CombatFullType;


    // 딕셔너리 초기화시 value값에 new 생성자를 하면 Key에 대한 Value는 이미 new로 처음 생성된 인스턴스를 재사용한것.
    // Func타입으로 함수로 new 생성자 처리 시 새 인스턴스
    private Dictionary<LocomotionStateType, Func<PlayerState>> m_locomotionStateCreateDic;
    private Dictionary<CombatFullStateType, Func<PlayerState>> m_combatStateCreateDic;

    private bool m_locomotionLocked;
    private bool m_combatLocked;
    public void LockLocomotion(bool value) => m_locomotionLocked = value;
    public void LockCombat(bool value) => m_combatLocked = value;
    public bool CanMove => !m_locomotionLocked && m_combatFlagsController.CurrentFlags == 0;
    public bool CanUseCombat => !m_combatLocked;

    public void InitializeMoudle(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
        m_combatFlagsController = playerCore.CombatFlagsController;
        m_locomotionStateCreateDic = new Dictionary<LocomotionStateType, Func<PlayerState>>
        { 
            // 일반 new로 작성시 상태가 현재 Dic 선언시에 객체로 저장이 되어져 그저 해당 상태를 재사용하는꼴임
            // Func을 통한 함수로써 객체를 만든다의 방식은 완전한 새로운 객체를 생성해내는 것
            {LocomotionStateType.Idle, ()=> new PlayerIdleState(m_playerCore) },
            {LocomotionStateType.Move, ()=> new PlayerMoveState(m_playerCore) },
            {LocomotionStateType.Jump, () => new PlayerJumpState(m_playerCore) },
            {LocomotionStateType.Fall, () => new PlayerFallState(m_playerCore) },
            {LocomotionStateType.FlyUp, () => new PlayerFlyUpState(m_playerCore) },
            {LocomotionStateType.Flying, () => new PlayerFlyingState(m_playerCore) },
        };
    }

    private void Start()
    {
        SwitchLocomotionState(LocomotionStateType.Idle);
    }

    private void Update()
    {
        m_locoState.Update();

        //m_combatFullState.Update();
    }

    public void SwitchLocomotionState(LocomotionStateType newState)
    {
        if (newState == m_locoType) return;
        Func<PlayerState> _newState = m_locomotionStateCreateDic[newState];
        m_locoState?.Exit();

        m_locoState = _newState();
        m_locoType = newState;

        m_locoState.Enter();
    }

    public void SwitchCombatFullState(CombatFullStateType newState)
    {
        if (newState == m_CombatFullType) return;
        Func<PlayerState> _newState = m_combatStateCreateDic[newState];
        m_combatFullState?.Exit();

        m_combatFullState = _newState();
        m_CombatFullType = newState;

        m_combatFullState.Enter();
    }
}
