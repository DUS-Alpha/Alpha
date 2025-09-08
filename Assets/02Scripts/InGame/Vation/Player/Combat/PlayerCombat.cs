using System;
using System.Linq;
using System.Security.Claims;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerInputHandler m_inputHandler;
    private CharacterController m_characterController;
    private PlayerAnimationController m_animationController;

    private Action<int> m_swapActions;

    // Locomotion 상태 제어를 위해
    public bool IsAction;
    public bool IsAttack { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsWeaponSwap => m_swapWeaponNum != CurrentWeaponNum && m_swapWeaponNum != 0;
    public int CurrentWeaponNum { get; private set; }
    private int m_swapWeaponNum;

    public void InitializeModule(PlayerInputHandler inputHandler, PlayerAnimationController animationController, CharacterController characterController)
    {
        m_inputHandler = inputHandler;
        m_animationController = animationController;
        m_characterController = characterController;
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
            IsAim = m_inputHandler.IsAim;
        }
        else if (CurrentWeaponNum == 3)
        {
            if (m_inputHandler.IsSniperScope)
            {
                IsAim = !IsAim;
            }
        }
    }
    // TODO : 전략패턴
    public void Attack(bool isAttack)
    {
        // 현재 무기에 따라 값 공격 방식 변경
        // TODO : Melee일 때 앞으로 살짝 이동이 필요할듯?
        m_animationController.AttackAni(isAttack);
    }

    public void Aiming()
    {

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


    public void OnAnimatorMove()
    {
        // Combat 공격 시 RootMotion에 대한 이동하는 포지션값을 애니메이션으로 받아와 실제로 이동시키기 위해
        if (m_animationController.IsRootMotion)
        {
            m_animationController.UpdateAnimatorTransformValue();
            // Animator가 계산한 이동량을 가져와서 CharacterController에 적용
            Vector3 _deltaPosition = m_animationController.RootMotionPos;
            //_deltaPosition.y = m_playerCore.Locomotion.BaseGravity * Time.deltaTime; // 중력 보정 (필요 시)

            m_characterController.Move(_deltaPosition);
            m_characterController.transform.rotation *= m_animationController.RootMotionRot;
        }
    }
}
