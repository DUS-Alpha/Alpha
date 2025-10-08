using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerCameraManger m_cameraManager;
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_animationController;
    private PlayerIKController m_ikController;
    private PlayerUIManager m_uiManager;
    private AudioManager m_audioManager;
    private InputLockedFlagsController<InputCombatLockType> m_flagsController;

    public bool IsSwap { get; private set; }
    private Action<int> m_swapAction;
    private int m_swapWeaponNum = 0;
    public int CurrentWeaponNum { get; private set; } = 0;
    public Weapon CurrentWeapon => currentWeapon;
    public Weapon currentWeapon => CurrentWeaponNum > 0 ? m_equipmentWeapons[CurrentWeaponNum] : null;

    public bool IsInCombat => m_isInCombat;
    private bool m_isInCombat;
    public bool IsAttack { get; private set; }
    private float m_nextAttakTime;
    public bool IsAim;
    private bool m_isAiming;
    public bool IsSkill { get; private set; }
    public string SkillKey;

    public bool IsReload { get; private set; }
    // 무기 관리
    
    private Weapon[] m_equipmentWeapons = new Weapon[4]; // 착용중인 무기

    public Queue<int> SkillQueue = new Queue<int>();
    public bool IsAction => m_isAction;
    public bool IsActioning;
    private bool m_isAction;
    public void InitializeModule(PlayerCore playerCore)
    {
        m_flagsController = playerCore.CombatFlagsController;
        m_equipmentWeapons = playerCore.EquipmentController.Weapons;
        m_cameraManager = playerCore.CameraManger;
        m_inputHandler = playerCore.InputHandler;
        m_animationController = playerCore.AniController;
        m_uiManager = playerCore.UIManager;
        m_equipmentWeapons = playerCore.EquipmentController.Weapons;
        m_ikController = playerCore.IKController;
        m_audioManager = playerCore.AudioManager;
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


    // 파라미터 Trigger형태는 KeyDown방식으로 최대한 관리
    public void CheckInput()
    {
        // TODO : Locomotion의 상태에 따른 Combat의 입력 bool값들 관리

        if (m_flagsController.HasFlag(InputCombatLockType.All))
        {
            IsAttack = false;
            IsSwap = false;
            IsReload = false;
            IsAim = false;
            IsSkill = false;
            return;
        }

        IsAttack = !m_flagsController.HasFlag(InputCombatLockType.Attack) 
                && m_inputHandler.IsAttack;

        IsSwap = !m_flagsController.HasFlag(InputCombatLockType.SwapWeapon) 
                && m_inputHandler.IsSwap && CanSwapWeapon();

        IsReload = !m_flagsController.HasFlag(InputCombatLockType.Reload) 
                && (CurrentWeaponNum > 1? m_inputHandler.IsReload : false);

        IsSkill = !m_flagsController.HasFlag(InputCombatLockType.Skill) 
                && m_inputHandler.IsSkill && !IsAction;

        IsAim = !m_flagsController.HasFlag(InputCombatLockType.Aim)
                && m_inputHandler.IsAim && CurrentWeaponNum > 1;
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
        m_isInCombat = true;
        m_animationController.SetIsInCombatAni(true);

        if (CurrentWeaponNum > 1)
        {
            SetIKRigWeight(1);
            m_animationController.SetAnimatorWeight(2, 1);
        }
        else 
            m_animationController.SetAnimatorWeight(1, 1);
    }

    /// <summary>
    /// Flying상태일때의 무기장착 Flying 애니메이션 없기에 상체 애니메이터 레이어 켜줘야함
    /// </summary>
    /// <param name="isFlying"></param>
    public void UpdateInCombat(bool isFlying)
    {
        if (CurrentWeaponNum != 1) return;

        if (isFlying) m_animationController.SetAnimatorWeight(1, 1);
        else
        {
            m_animationController.SetAnimatorWeight(3, 1);
        }
    }

    public void ExitInCombat()
    {
        m_isInCombat = false;
        m_isAction = false;
        IsActioning = false;

        m_animationController.SetIsInCombatAni(false);

        m_animationController.SetAnimatorWeight(1, 0);
        m_animationController.SetAnimatorWeight(2, 0);
        m_animationController.SetAnimatorWeight(3, 0);
        
        SetIKRigWeight(0);
    }
    public void SetAttack(bool isAttack)
    {
        m_animationController.SetAttackAni(isAttack);
    }
    public void Attack()
    {
        if (CurrentWeaponNum == 0) return;
        MeleeWeapon _meleeWeapon = null;
        RangeWeapon _rangeWeapon = null;

        if (CurrentWeaponNum == 1)
        {
            _meleeWeapon = CurrentWeapon as MeleeWeapon;
            if(!IsActioning)
            {
                m_animationController.MeleeComboTriggerAni();
            }
        }
        else if (CurrentWeaponNum > 1)
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

    /// <summary>
    /// Player오브젝트 하위에 있는 각 Holder 오브젝트 On/Off 방식
    /// TODO 스왑시 스왑상태에서 시간에 의해 애니메이션 Num값과 실제 Swap값이 다르게 가~끔나옴 해결필요
    /// </summary>
    private bool CanSwapWeapon()
    {
        int _swapNum = m_inputHandler.SwapWeaponNum;
        // 같은 번호 입력시 리턴
        if (_swapNum == CurrentWeaponNum) return false;
        // 무기가 없을경우 리턴
        if (m_equipmentWeapons[_swapNum] == null) return false;

        CurrentWeaponNum = _swapNum;
        return true;
    }

    public void EnterSwapWeapon(bool isFlying)
    {
        // 무기 교체
        m_swapAction?.Invoke(CurrentWeaponNum);

        SetIKRigWeight(0);
        m_animationController.SetAnimatorWeight(2,1);
        m_animationController.SwapWeaponAni(CurrentWeaponNum, isFlying);

        if (CurrentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
        else m_uiManager.SetAmmo(0, 0, 0);
    }

    public void ExitSwapWeapon()
    {
        if (CurrentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
        else m_uiManager.SetAmmo(0, 0, 0);
        m_animationController.SetAnimatorWeight(2, 0);
    }

    // TODO : 정리 후 고려
    /*public void AttackRootMotion(bool isApplyRoot)
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
    }*/

    public void SetAming(bool isAim, bool isOffAiming = false)
    {
        if (isAim) m_isAiming = !m_isAiming;
        if (isOffAiming) m_isAiming = false;
        int _isAimWeight = m_isAiming ? 1 : 0;
        
        // TODO : UpperBody라서 Fly일때도 같이 쓰기에 다시 조율 필요
        m_cameraManager.AimFOV(m_isAiming, CurrentWeaponNum);
    }

    public bool EnterReload()
    {
        RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
        bool _cansReload = _rangeWeapon.Reload();
        if(!_cansReload) return false;

        m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);

        m_animationController.SetAnimatorWeight(2, 1);
        m_animationController.ReloadAni();
        m_audioManager.PlaySFXCombatAudio(SFXCombatType.Reload);
        return true;

    }
    public void ExitReload()
    {
        m_animationController.SetAnimatorWeight(2, 0);
    }

    public void EnterSkill()
    {
        SkillKey = m_inputHandler.SkillKey;
        m_animationController.SkillAni(SkillKey);
    }
    #endregion ================================================ /Enter,Exit State
}
