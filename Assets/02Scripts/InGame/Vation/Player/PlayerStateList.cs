using System;
using UnityEngine;

// 최상위 FSM과 서브 FSM으로 관리

// Locomotion : 이동/기본 조작
public enum LocomotionStateType
{
    None,
    Idle,
    Move,
    Jump,
    Fall,
    Land,
    FlyUp,
    FlightMove,
    Dash,
    Die
}

// Combat : 전투
// Upper_없는것은 AllBodyLayer
public enum CombatStateType
{
    None,
    NonCombat,
    Upper_InCombat,
    Upper_SwapWeapon,
    Upper_Reload,
    Skill,
    Dodge
}

/*// Flags enum 형태는 비트연산 처리 필요
[Flags]
public enum SubFlagsStateTpye
{
    None = 0,
    Aim = 1 << 0,
    RangeShoot = 1 << 1,
    SwapWeapon = 1 << 2,
    Reload = 1 << 3,
}
*/