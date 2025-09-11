using System;
using UnityEngine;

public enum LocomotionStateType
{
    None,
    Idle,
    Move,
    Jump,
    Fall,
    Landing,
    FlyUp,
    Flying,
    Die
}

// Flags enum 형태는 비트연산 처리 필요
[Flags]
public enum CombatFlagsStateTpye
{
    None = 0,
    Aim = 1 << 0,
    RangeShoot = 1 << 1,
    SwapWeapon = 1 << 2,
    Reload = 1 << 3,
}

public enum CombatFullStateType
{
    None,
    MeleeAttack,
    Dodge,
    RangeSkill,
    MeleeSkill
}