using UnityEngine;

public class CombatFlagsController
{
    public CombatFlagsStateTpye CurrentFlags => m_currentFlags;
    private CombatFlagsStateTpye m_currentFlags;
    private CombatFlagsStateTpye m_triggerFlagsNextFrame;   // Trigger 파라미터 애니메이션은 1프레임 시간 잠깐줬다가 바로 삭제(삭제는 필요하지만 애니메이터 파라미터값 받아지는 시간 필요하기에)

    // TODO : 우선 CombatFlags들 단수개로만 존재하도록 설정 향후 복수개도 가능하게
    private CombatFlagsStateTpye m_exclusiveFlags =
         CombatFlagsStateTpye.Aim
        | CombatFlagsStateTpye.Reload
        | CombatFlagsStateTpye.SwapWeapon
        | CombatFlagsStateTpye.RangeShoot;

    private bool IsTrigger(CombatFlagsStateTpye flag) =>
        flag == CombatFlagsStateTpye.Reload ||
        flag == CombatFlagsStateTpye.SwapWeapon ||
        flag == CombatFlagsStateTpye.RangeShoot;

    /// <summary>
    /// 애니메이터 파라미터가 Trigger는 추가후 바로 제거됨
    /// </summary>
    /// <param name="flags"></param>
    public void AddFlag(CombatFlagsStateTpye flag)
    {
        // 1. 단독 Flags사용을 위한 기존 Exclusive Flags 모두 제거
        if ((m_exclusiveFlags & flag) != 0)
        {
            // 기존에 걸려있던 다른 ExclusiveFlag들을 전부 제거
            m_currentFlags &= ~m_exclusiveFlags;
        }

        if (IsTrigger(flag))
            m_triggerFlagsNextFrame |= flag;
        else
            m_currentFlags |= flag;

        if (m_triggerFlagsNextFrame != 0)
        {
            m_currentFlags |= m_triggerFlagsNextFrame;
        }
    }

    public void RemoveFlag(CombatFlagsStateTpye flag)
    {
        m_currentFlags &= ~flag;
    }
    public bool HasFlag(CombatFlagsStateTpye flag) => m_currentFlags.HasFlag(flag);
    public void ClearCombatFlags() => m_currentFlags = CombatFlagsStateTpye.None;

    public void TriggerClear()
    {
        // 다음 프레임에 트리거 플래그 제거
        m_currentFlags &= ~m_triggerFlagsNextFrame;

        m_triggerFlagsNextFrame = 0; // 다음 프레임 제거
    }
}
