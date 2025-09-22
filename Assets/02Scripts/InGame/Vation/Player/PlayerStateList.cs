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
    Landing,
    Dodge,
    FlyUp,
    Flying,
    Die,
}

// Combat : 전투
// 주로 UpperBody 레이어 사용
public enum CombatStateType
{
    None,
    Idle,
    Aim,
    Attack,
    SwapWeapon,
    Reload,
    MeleeSkill_All,
    RangeSkill_All,
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