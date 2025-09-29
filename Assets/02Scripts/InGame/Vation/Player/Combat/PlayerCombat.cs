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

    public bool IsCombat { get; private set; }
    public bool IsCombating => m_isCombating;
    private bool m_isCombating;
    public bool IsAttack { get; private set; }
    public bool IsScope => m_isScope;
    private bool m_isScope;
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
    public void InitializeModule(PlayerCore playerCore)
    {
        m_cameraManager = playerCore.CameraManger;
        m_inputHandler = playerCore.InputHandler;
        m_animationController = playerCore.AniController;
        m_uiManager = playerCore.UIManager;
        m_equipmentWeapons = playerCore.EquipmentController.Weapons;
        m_ikController = playerCore.IKController;
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }

    /// <summary>
    /// PlayerCore에서 옵저버패턴으로 받은 SwapAction을 받아옴
    /// </summary>
    /// <param name="swapAction"></param>
    public void SetSwapAction(Action<int> swapAction)
    {
        // 매개변수가 아니라 m_swapAction = m_playerCOre.SwapAction으로 받아오려니 안됨
        m_swapAction = swapAction;
    }


    private void Start()
    {
        m_currentWeaponNum = 0;
    }

    // 파라미터 Trigger형태는 KeyDown방식으로 최대한 관리
    public void CheckInput()
    {
        IsAttack = m_inputHandler.IsAttack;
        bool _isScope = m_inputHandler.IsScope;
        m_swapWeaponNum = m_inputHandler.SwapWeaponNum;
        IsReload = m_inputHandler.IsReload;

        // Toggle 형태로
        if(_isScope)
        {
            m_isScope = !m_isScope;
        }

        IsCombat = IsAttack || _isScope;
    }
    public void SetIsAction(bool isAction)
    {
        m_isAction = isAction;
    }
    public void SetIKRigWeight(int weight)
    {
        m_ikController.SetRigWeight(RigType.Aim, weight);
    }
    // UpperLayer
    public void SetUpperAnimatorLayer(int weight)
    {
        m_animationController.SetAnimatorWeight(1, weight);
    }

    #region ================================================ Enter, Exit State
    public void EnterInCombat()
    {
        m_isCombating = true;
        m_animationController.SetIsCombatAni(m_isCombating);
        if (m_currentWeaponNum > 1) SetIKRigWeight(1);
    }
    public void ExitInCombat()
    {
        m_isCombating = false;
        m_animationController.SetIsCombatAni(m_isCombating);
        SetIKRigWeight(0);
    }
    
    public void Attack()
    {
        if (m_currentWeaponNum == 0) return;
        MeleeWeapon _meleeWeapon = null;
        RangeWeapon _rangeWeapon = null;

        if (m_currentWeaponNum == 1)
        {
            _meleeWeapon = CurrentWeapon as MeleeWeapon;
        }
        else if (m_currentWeaponNum > 1)
        {
            _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }

        if (Time.time >= m_nextAttakTime)
        {
            m_nextAttakTime = Time.time + currentWeapon.WeaponData.AttackDelay;
            // 무기 Swap시 마다 스나이퍼 같은 총의 경우 바로 발사를 하면 안되기에 계속 현재 무기값으로
            currentWeapon.Attack(IsAttack, m_animationController);
        }
    }
    public void EnterSwapWeapon()
    {
        m_isAction = true;
        // 옵저버 패턴 - 각 모듈의 액션들 처리
        // 현재는 PlayerEquipmentController의 SwapAction함수만 저장
        
        m_swapAction?.Invoke(m_currentWeaponNum);
        m_animationController.SetAnimatorWeight(2,1);
        m_animationController.SwapWeaponAni(m_currentWeaponNum);
    }

    public void ExitSwapWeapon()
    {
        m_isAction = false;
        if (m_currentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
        m_animationController.SetAnimatorWeight(2, 0);
    }
    /// <summary>
    /// Player오브젝트 하위에 있는 각 Holder 오브젝트 On/Off 방식
    /// TODO 스왑시 스왑상태에서 시간에 의해 애니메이션 Num값과 실제 Swap값이 다르게 가~끔나옴 해결필요
    /// </summary>
    public bool IsSwapWeapon()
    {
        // 같은 번호 입력시 리턴
        if (m_swapWeaponNum == 0 || m_swapWeaponNum == CurrentWeaponNum)
        {
            return false;
        }
        // 무기가 없을경우 리턴
        if (m_equipmentWeapons[m_swapWeaponNum] == null) return false;
        m_currentWeaponNum = m_swapWeaponNum;
        return true;
    }


    public void AttackRootMotion(bool isApplyRoot)
    {
       // m_animationController.SetApplyRootMotion(isApplyRoot);
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
    
    public void ExitAttack()
    {
        m_animationController.AttackAni(false, m_currentWeaponNum);
        
        m_animationController.SetAnimatorWeight(2, 0);
    }

    public void SetAming(bool isAim)
    {
        int _isAimWeight = isAim ? 1 : 0;
        
        // TODO : UpperBody라서 Fly일때도 같이 쓰기에 다시 조율 필요
        m_cameraManager.AimFOV(m_isScope, m_currentWeaponNum);
    }

    public bool EnterReload()
    {
        m_isAction = true;
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
        m_isAction = false;
        m_isReloading = false;
        m_animationController.SetAnimatorWeight(2, 0);
    }
    #endregion ================================================ /Enter,Exit State
}
