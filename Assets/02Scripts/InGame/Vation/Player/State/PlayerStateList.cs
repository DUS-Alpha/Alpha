using System;
using UnityEngine;

// 최상위 FSM과 서브 FSM으로 관리

// Locomotion : 이동/기본 조작
// TODO : 
public enum LocomotionStateType
{
    None,
    Idle,
    Move,
    Jump,
    Fall,
    Land,
    VerticalTakeOff,
    FlightMove,
    Die
}

// Combat : 전투
// Upper_없는것은 AllBodyLayer
public enum CombatStateType
{
    None,
    NonCombat,
    Swap,
    Attack,
    CombatReady,
    Skill,
}