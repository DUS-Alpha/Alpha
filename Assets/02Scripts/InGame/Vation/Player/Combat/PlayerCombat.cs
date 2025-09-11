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

    public bool IsAttack { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsWeaponSwap { get; private set; }
    public bool IsReload { get; private set; }
    public int CurrentWeaponNum { get; private set; }
    private Weapon m_currentWeapon;

    private int m_swapWeaponNum;

    private CombatFlagsStateTpye m_prevCombat;
    private CombatFlagsController m_flagsController;
    public void InitializeModule(PlayerInputHandler inputHandler, CombatFlagsController flagsController, PlayerAnimationController animationController)
    {
        m_inputHandler = inputHandler;
        //m_playerStateMachine = stateMachine;
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
    public void CheckInput()
    {
        //IsAttack = m_inputHandler.IsAttack;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
       
        SwapWeapon();

        if (CurrentWeaponNum == 2)
        {
            IsAim = m_inputHandler.IsAim || m_inputHandler.IsAttack;
            IsReload = m_inputHandler.IsReload;
        }
        else if (CurrentWeaponNum == 3)
        {
            if (m_inputHandler.IsSniperScope)
            {
                IsAim = !IsAim;
            }
            IsReload = m_inputHandler.IsReload;
        }
        else
        {
            IsAim = false;
            IsReload = false;
        }

        if (IsAim)
            m_flagsController.AddFlag(CombatFlagsStateTpye.Aim);
        else
            m_flagsController.RemoveFlag(CombatFlagsStateTpye.Aim);

        if(IsWeaponSwap)
        {
            m_flagsController.AddFlag(CombatFlagsStateTpye.SwapWeapon); // 애니메이션 파라미터 Trigger이기에 자동 Remove
        }

        m_animationController.UpdateCombatFlagsAnimations(m_flagsController.CurrentFlags);

        m_flagsController.TriggerClear();
    }

    /// <summary>
    /// 현재는 단순 Holder만 OnOff
    /// </summary>
    public void SwapWeapon()
    {
        if (m_swapWeaponNum == 0 || m_swapWeaponNum == CurrentWeaponNum)
        {
            IsWeaponSwap = false;
            return;
        }
        CurrentWeaponNum = m_swapWeaponNum;
        IsWeaponSwap = true;
        // 각 모듈의 액션들 처리
        m_swapAction?.Invoke(m_swapWeaponNum);
    }
}
