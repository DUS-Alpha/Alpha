using System;
using System.Linq;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerCameraManger m_cameraManager;
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_animationController;

    private Action<int> m_swapAction;

    public bool IsMeleeAttack { get; private set; }
    public bool IsRangeShooting { get; private set; }
    public bool IsAim { get; private set; }
    public bool IsSwapWeapon { get; private set; }
    public bool IsReload { get; private set; }

    // 무기 관리
    public int CurrentWeaponNum => m_currentWeaponNum;
    private int m_currentWeaponNum;
    public Weapon CurrentWeapon => m_currentWeapon;
    private Weapon m_currentWeapon => m_currentWeaponNum > 0 ? m_equipmentWeapons[m_currentWeaponNum] : null;
    private int m_swapWeaponNum;
    private Weapon[] m_equipmentWeapons = new Weapon[4]; // 착용중인 무기
    private float m_weaponNextFire = 0f; // Weapon 발사 가능 시간
    private float m_weaponDelay;
    public void InitializeModule(PlayerCameraManger cameraManager, PlayerInputHandler inputHandler, PlayerAnimationController animationController, Weapon[] weapons)
    {
        m_cameraManager = cameraManager;
        m_inputHandler = inputHandler;
        m_animationController = animationController;
        m_equipmentWeapons = weapons;
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
        m_currentWeaponNum = 0;
    }

    // 파라미터 Trigger형태는 KeyDown방식으로 최대한 관리
    public void CheckInput()
    {
        IsMeleeAttack = m_inputHandler.IsMeleeAttack;
        IsRangeShooting = m_inputHandler.IsRangeShooting;
        IsAim = m_inputHandler.IsAim || IsRangeShooting;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
        IsReload = m_inputHandler.IsReload;
        SwapWeapon();
    }

    /// <summary>
    /// Player오브젝트 하위에 있는 각 Holder 오브젝트 On/Off 방식
    /// TODO 스왑시 스왑상태에서 시간에 의해 애니메이션 Num값과 실제 Swap값이 다르게 가~끔나옴 해결필요
    /// </summary>
    public void SwapWeapon()
    {
        // 같은 번호 입력시 리턴
        if (m_swapWeaponNum == 0 || m_swapWeaponNum == CurrentWeaponNum)
        {
            IsSwapWeapon = false;
            return;
        }
        // 무기가 없을경우 리턴
        if (m_equipmentWeapons[m_swapWeaponNum] == null) return;
        IsSwapWeapon = true;
    }

    public void EnterSwapWeapon()
    {
        // 옵저버 패턴 - 각 모듈의 액션들 처리
        // 현재는 PlayerEquipmentController의 SwapAction함수만 저장
        m_currentWeaponNum = m_swapWeaponNum;
        m_swapAction?.Invoke(m_swapWeaponNum);
        m_animationController.SwapWeaponAni(m_currentWeaponNum);
    }

    public void AttackRootMotion(bool isApplyRoot)
    {
        m_animationController.SetApplyRootMotion(isApplyRoot);
    }

    public void SetNextAttackDelay(float value)
    {
        m_weaponDelay = value;
    }
    public void Attack()
    {
        if(m_currentWeaponNum == 0) return;

        // 각 무기에 따라 AttackInput적용
        bool _isAttack = (m_currentWeapon is MeleeWeapon) ? m_inputHandler.IsMeleeAttack : m_inputHandler.IsRangeShooting;
        if (Time.time >= m_weaponNextFire)
        {
            m_currentWeapon.Attack(_isAttack, m_animationController);
            m_weaponNextFire = Time.time + Mathf.Max(m_currentWeapon.WeaponData.AttackDelay, 0f);
        }
        
    }

    public void Aming(bool isAim)
    {
        m_cameraManager.AimFOV(isAim);
        m_animationController.AimAni(isAim);
    }
}
