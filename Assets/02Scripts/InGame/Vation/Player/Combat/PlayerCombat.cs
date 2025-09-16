using System;
using System.Linq;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_animationController;
    private PlayerStateMachine m_playerStateMachine;
    private Action<int> m_swapAction;

    public bool IsMeleeAttack { get; private set; }
    public bool IsRangeShooting { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsSwapWeapon { get; private set; }
    public bool IsReload { get; private set; }
    public int CurrentWeaponNum { get; private set; }
    private Weapon m_currentWeapon;

    private int m_swapWeaponNum;

    public bool IsGoIdle {  get; private set; }
    private bool m_isGoIdle;
    private InputLockedFlagsController<InputLocoLockType> m_flagsController;
    public void InitializeModule(PlayerInputHandler inputHandler, InputLockedFlagsController<InputLocoLockType> flagsController, PlayerAnimationController animationController)
    {
        m_inputHandler = inputHandler;
        m_animationController = animationController;
        m_flagsController = flagsController;
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }

    public void SetSwapAction(Action<int> swapAction)
    {
        m_swapAction = swapAction;
    }
    /// <summary>
    /// PlayerCore에서 옵저버패턴으로 받은 Swap관련된 Action을 Combat에서 관리
    /// </summary>
    /// <param name="swapAction"></param>

    private void Start()
    {
        CurrentWeaponNum = 0;
    }

    // 파라미터 Trigger형태는 KeyDown방식으로 최대한 관리
    public void CheckInput()
    {
        IsMeleeAttack = m_inputHandler.IsMeleeAttack;
        IsRangeShooting = m_inputHandler.IsRangeShooting;
        IsAim = m_inputHandler.IsAim;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
        IsReload = m_inputHandler.IsReload;

        SwapWeapon();
    }
    public void GoIdle()
    {
        m_isGoIdle = true;
    }

    public void UpdateFlagsState()
    {
        /*if (IsAim)
            m_flagsController.AddFlag(SubFlagsStateTpye.Aim);
        else
            m_flagsController.RemoveFlag(SubFlagsStateTpye.Aim);

        if (IsWeaponSwap)
        {
            m_flagsController.AddFlag(SubFlagsStateTpye.SwapWeapon); // 애니메이션 파라미터 Trigger이기에 자동 Remove
        }

        if (IsReload)
        {
            m_flagsController.AddFlag(SubFlagsStateTpye.Reload);
        }

        m_animationController.UpdateCombatFlagsAni(m_flagsController.CurrentFlags);

        m_flagsController.TriggerClear();*/
    }

    public void EnterSwapWeapon()
    {
        m_animationController.SwapWeaponAni(CurrentWeaponNum);
    }
    /// <summary>
    /// 현재는 단순 Holder만 OnOff
    /// </summary>
    public void SwapWeapon()
    {
        if (m_swapWeaponNum == 0 || m_swapWeaponNum == CurrentWeaponNum)
        {
            IsSwapWeapon = false;
            return;
        }
        CurrentWeaponNum = m_swapWeaponNum;
        IsSwapWeapon = true;

        // 각 모듈의 액션들 처리(현재는 인벤토리 및 뷰에서의 변경 처리)
        m_swapAction?.Invoke(m_swapWeaponNum);
    }
}
