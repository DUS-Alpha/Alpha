using System;
using System.Linq;
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
    public bool IsWeaponSwap { get; private set; }
    public int m_currentWeaponNum;
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
    public void SetSwapAction(Action<int> swapAction)
    {
        m_swapActions = swapAction;
    }

    private void Start()
    {
        m_currentWeaponNum = 0;
    }
    public void CheckInput()
    {
        IsAttack = m_inputHandler.IsAttack;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
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
    /// 상태를 가지는것이 아닌 애니메이터 Layer로 관리 (특정 동작 제외 모든 상태에서 진행이 되도록)
    /// </summary>
    public void SwapWeapon()
    {
        if (m_swapWeaponNum == m_currentWeaponNum) return;
        //if (IsWeaponSwap) return;

        //IsWeaponSwap = true;
        m_currentWeaponNum = m_swapWeaponNum;

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
