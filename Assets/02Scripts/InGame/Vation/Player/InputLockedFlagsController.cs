using System;
using UnityEngine;

// 상태에 따른 입력 잠금 타입 복수개 선택
[Flags]
public enum InputLocoLockType
{
    None = 0,
    Move = 1 << 0,    // 이동 관련 입력
    Look = 1 << 1,        // 카메라 회전
    Jump = 1 << 2,
    Dodge = 1 << 3,       // 회피 입력
    FlyUp = 1 << 4,
    FlyOff = 1 << 5,
    All = 1 << 6,
}

[Flags]
public enum InputCombatLockType
{
    None = 0,
    Aim = 1 << 0,
    Attack = 1 << 1,
    MeleeAttack = 1 << 2,
    RangeShooting = 1 << 3,
    SwapWeapon = 1 << 4,
    Reload = 1 << 5,
    Skill = 1 << 6,
    All = 1 << 7,
}

public class InputLockedFlagsController<T> where T : Enum
{
    public T CurrentFlags => m_flags;
    private T m_flags;

    public void AddLockedFlag(T flag)
    {
        if (flag.Equals(default(T))) return; // None 처리
        m_flags = (T)(object)(((int)(object)m_flags) | ((int)(object)flag));
    }

    public void AddAllFlags()
    {
        int allFlags = 0;
        foreach (var flag in Enum.GetValues(typeof(T)))
        {
            // default(T)는 None일 가능성이 높으므로 건너뜀
            if (!flag.Equals(default(T)))
                allFlags |= (int)flag;
        }

        m_flags = (T)(object)allFlags;
    }

    public void RemoveLockedFlag(T flag)
    {
        if (flag.Equals(default(T))) return;
        m_flags = (T)(object)(((int)(object)m_flags) & ~((int)(object)flag));
    }
    public bool HasFlag(T flag)
    {
        return ((int)(object)m_flags & (int)(object)flag) != 0;
    }

    public void ClearAllFlags()
    {
        m_flags = default(T);
    }
}
