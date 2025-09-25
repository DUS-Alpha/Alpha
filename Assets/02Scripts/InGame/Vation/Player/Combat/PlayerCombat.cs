using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerCameraManger m_cameraManager;
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_animationController;
    private PlayerIKController m_ikController;
    private PlayerUIManager m_uiManager;
    private Action<int> m_swapAction;
    public bool IsAttack { get; private set; }
    public bool IsAiming => m_isAiming;
    private bool m_isAiming;
    public bool IsSwapWeapon { get; private set; }
    public bool IsReload { get; private set; }
    public bool m_isReloading;
    // 무기 관리
    public int CurrentWeaponNum => m_currentWeaponNum;
    private int m_currentWeaponNum;
    public Weapon CurrentWeapon => currentWeapon;
    public Weapon currentWeapon => m_currentWeaponNum > 0 ? m_equipmentWeapons[m_currentWeaponNum] : null;
    private int m_swapWeaponNum;
    private Weapon[] m_equipmentWeapons = new Weapon[4]; // 착용중인 무기

    private float m_nextAttakTime;

    public bool IsAction => m_isAction;
    private bool m_isAction;
    public void InitializeModule(PlayerCameraManger cameraManager, 
        PlayerInputHandler inputHandler, 
        PlayerAnimationController animationController, 
        Weapon[] weapons, PlayerIKController IKController,
        PlayerUIManager uiManager)
    {

        m_cameraManager = cameraManager;
        m_inputHandler = inputHandler;
        m_animationController = animationController;
        m_uiManager = uiManager;
        m_equipmentWeapons = weapons;
        m_ikController = IKController;
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
        IsAttack = m_inputHandler.IsAttack;
        bool _isAim = m_inputHandler.IsAim;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
        IsReload = m_inputHandler.IsReload;

        if(_isAim)
        {
            m_isAiming = !m_isAiming;
        }

        if (m_isReloading)
        {
            IsAttack = false;
            m_isAiming = false;
        }

        CheckCanSwapWeapon();
    }

    public void EnterSwapWeapon()
    {
        // 옵저버 패턴 - 각 모듈의 액션들 처리
        // 현재는 PlayerEquipmentController의 SwapAction함수만 저장
        m_animationController.SetAnimatorWeight(2,1);
        m_currentWeaponNum = m_swapWeaponNum;
        m_swapAction?.Invoke(m_swapWeaponNum);
        m_animationController.SwapWeaponAni(m_currentWeaponNum);
    }

    public void ExitSwapWeapon()
    {
        m_animationController.SetAnimatorWeight(2, 0);
        if (m_currentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
    }
    /// <summary>
    /// Player오브젝트 하위에 있는 각 Holder 오브젝트 On/Off 방식
    /// TODO 스왑시 스왑상태에서 시간에 의해 애니메이션 Num값과 실제 Swap값이 다르게 가~끔나옴 해결필요
    /// </summary>
    public void CheckCanSwapWeapon()
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


    public void AttackRootMotion(bool isApplyRoot)
    {
        m_animationController.SetApplyRootMotion(isApplyRoot);
    }

    // Melee Animation에서 불러오는중
    public void OnWeaponCollider()
    {
        MeleeWeapon _meleeWeapon = currentWeapon as MeleeWeapon;
        _meleeWeapon.SetActivateCollider(true);
    }
    public void OffWeaponCollider()
    {
        MeleeWeapon _meleeWeapon = currentWeapon as MeleeWeapon;
        _meleeWeapon.SetActivateCollider(false);
    }
    // AttackUpdate
    public void Attack()
    {
        if (m_currentWeaponNum == 0) return;
        MeleeWeapon _meleeWeapon = null;
        RangeWeapon _rangeWeapon = null;

        if(m_currentWeaponNum == 1)
        {
            _meleeWeapon = CurrentWeapon as MeleeWeapon;
        }
        else if(m_currentWeaponNum > 1)
        {
            _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }

        if (Time.time >= m_nextAttakTime)
        {
            m_nextAttakTime = Time.time + currentWeapon.WeaponData.AttackDelay;
            // 무기 Swap시 마다 스나이퍼 같은 총의 경우 바로 발사를 하면 안되기에 계속 현재 무기값으로
            currentWeapon.Attack(IsAttack, m_animationController);
            m_animationController.SetAnimatorWeight(2, 1f);
        }
    }
    public void ExitAttack()
    {
        m_animationController.AttackAni(false, m_currentWeaponNum);

        if(m_currentWeaponNum == 1)
        {
            m_animationController.SetAnimatorWeight(2, 0);
        }
    }

    public void SetIsAction(bool isAction)
    {
        m_isAction = isAction;
    }

    public void SetAming(bool isAim, bool isFlying = false)
    {
        int _isAimWeight = isAim ? 1 : 0;
        m_ikController.SetWeight(RigType.Aim, _isAimWeight);
        
        // TODO : UpperBody라서 Fly일때도 같이 쓰기에 다시 조율 필요
        
        m_cameraManager.AimFOV(m_isAiming, m_currentWeaponNum);
        m_animationController.AimAni(isAim);
    }

    public bool EnterReload()
    {
        RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
        bool _cansReload = _rangeWeapon.Reload();
        if(!_cansReload) return false;

        m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        m_isReloading = true;
        m_animationController.SetAnimatorWeight(2, 1);
        m_animationController.ReloadAni();
        return true;

    }
    public void ExitReload()
    {
        m_isReloading = false;
        m_animationController.SetAnimatorWeight(2, 0);
    }
}
