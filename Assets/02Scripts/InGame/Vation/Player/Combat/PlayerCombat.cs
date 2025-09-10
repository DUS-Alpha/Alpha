using System;
using System.Linq;
using System.Security.Claims;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_animationController;
    private PlayerCameraManger m_cameraManager;

    private Action<int> m_swapActions;

    // Locomotion 상태 제어를 위해
    public bool IsCombatProgressing;
    public bool IsAttack { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsWeaponSwap => m_swapWeaponNum != CurrentWeaponNum && m_swapWeaponNum != 0;
    public int CurrentWeaponNum { get; private set; }
    private int m_swapWeaponNum;

    public void InitializeModule(PlayerInputHandler inputHandler, PlayerAnimationController animationController, PlayerCameraManger cameraManger)
    {
        m_inputHandler = inputHandler;
        m_animationController = animationController;
        m_cameraManager = cameraManger;
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }

    /// <summary>
    /// PlayerCore에서 옵저버패턴으로 받은 Swap관련된 Action을 Combat에서 관리
    /// </summary>
    /// <param name="swapAction"></param>
    public void SetSwapAction(Action<int> swapAction)
    {
        m_swapActions = swapAction;
    }

    private void Start()
    {
        CurrentWeaponNum = 0;
    }
    public void CheckInput()
    {
        IsAttack = m_inputHandler.IsAttack;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
        
        if (CurrentWeaponNum == 2)
        {
            IsAim = m_inputHandler.IsAim || m_inputHandler.IsAttack;
        }
        else if (CurrentWeaponNum == 3)
        {
            if (m_inputHandler.IsSniperScope)
            {
                IsAim = !IsAim;
            }
        }
        else
        {
            IsAim = false;
        }
    }

    public void SetIsAllBodyAction(bool isAllBodyAction)
    {
        IsCombatProgressing = isAllBodyAction;
    }

    public void CheckAreaTarget()
    {

    }
    // TODO : 전략패턴
    public void Attack(bool isAttack)
    {
        // 현재 무기에 따라 값 공격 방식 변경
        // TODO : Melee일 때 앞으로 살짝 이동이 필요할듯?
        m_animationController.AttackAni(isAttack, IsCombatProgressing);
    }
    public void Aiming(bool isAim)
    {
        m_animationController.AimAni(isAim);
        if(isAim)
        {

        }
    }

    /// <summary>
    /// 현재는 단순 Holder만 OnOff
    /// </summary>
    public void SwapWeapon()
    {
        CurrentWeaponNum = m_swapWeaponNum;

        // 각 모듈의 액션들 처리
        m_swapActions?.Invoke(m_swapWeaponNum);
    }
}
