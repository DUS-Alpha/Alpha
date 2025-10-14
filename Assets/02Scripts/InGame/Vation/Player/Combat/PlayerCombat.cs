using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Ref Component
    private PlayerCameraManger m_cameraManager;
    private PlayerInputHandler m_inputHandler;
    private PlayerAnimationController m_aniController;
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
    public bool IsAiming => m_isAiming;
    private bool m_isAiming;
    public bool IsSkill { get; private set; }
    public string SkillKey;

    public bool IsReload { get; private set; }
    // 무기 관리
    
    private Weapon[] m_equipmentWeapons = new Weapon[4]; // 착용중인 무기

    public Queue<int> SkillQueue = new Queue<int>();
    public bool IsAction;
    public bool IsActioning;
    private bool m_isAttackDistance;
    public void InitializeModule(PlayerCore playerCore)
    {
        m_flagsController = playerCore.CombatFlagsController;
        m_equipmentWeapons = playerCore.EquipmentController.Weapons;
        m_cameraManager = playerCore.CameraManger;
        m_inputHandler = playerCore.InputHandler;
        m_aniController = playerCore.AniController;
        m_uiManager = playerCore.UIManager;
        m_equipmentWeapons = playerCore.EquipmentController.Weapons;
        m_ikController = playerCore.IKController;
        m_audioManager = playerCore.AudioManager;
    }

    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }
    private void Update()
    {
        if(currentWeapon == null) return;
        m_isAttackDistance = TryGetTarget(out RaycastHit hit, currentWeapon.m_maxDistance);
        SetColorMarkCrossHeadUI(m_isAttackDistance);
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
                && m_inputHandler.IsAttack && CurrentWeaponNum > 0;

        IsSwap = !m_flagsController.HasFlag(InputCombatLockType.SwapWeapon) 
                && m_inputHandler.IsSwap && CanSwapWeapon();

        IsReload = !m_flagsController.HasFlag(InputCombatLockType.Reload) 
                 && m_inputHandler.IsReload && CurrentWeaponNum > 1;
        IsAim = !m_flagsController.HasFlag(InputCombatLockType.Aim)
                && m_inputHandler.IsAim && CurrentWeaponNum > 1;

        IsSkill = !m_flagsController.HasFlag(InputCombatLockType.Skill) 
                & m_inputHandler.IsSkill && CurrentWeaponNum == 1;
    }
    public void SetIsAction(bool isAction)
    {
        IsAction = isAction;
    }
    public void SetIKRigWeight(RigType rigType,bool isWeight)
    {
        m_ikController.SetRigWeight(rigType, isWeight);
    }



    #region ================================================ Enter, Exit State
    public void EnterInCombat()
    {
        m_isInCombat = true;
        m_aniController.SetIsInCombatAni(m_isInCombat);
    }

    public void ExitInCombat(bool isFlying)
    {
        m_isInCombat = false;
        IsActioning = false;

        m_aniController.SetIsInCombatAni(false);

        if(!isFlying)
        m_aniController.SetAnimatorWeight(1, 0);
        
        m_aniController.SetAnimatorWeight(2, 0);
        m_aniController.SetAnimatorWeight(3, 0);
    }

    public void Attack()
    {
        MeleeWeapon _meleeWeapon = null;
        RangeWeapon _rangeWeapon = null;

        if (CurrentWeaponNum == 1)
        {
            m_aniController.SetAnimatorWeight(2, 1);
            SetIKRigWeight(RigType.Aim, false);
            _meleeWeapon = CurrentWeapon as MeleeWeapon;
            
            if (!IsActioning)
            {
                currentWeapon.Attack(IsAttack, m_aniController);
            }
        }
        else if (CurrentWeaponNum > 1)
        {
            SetIKRigWeight(RigType.Aim, true);
            _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
            m_aniController.SetAnimatorWeight(1, 1);
            if (Time.time >= m_nextAttakTime)
            {
                m_nextAttakTime = Time.time + currentWeapon.WeaponData.AttackDelay;
                // 무기 Swap시 마다 스나이퍼 같은 총의 경우 바로 발사를 하면 안되기에 계속 현재 무기값으로
                _rangeWeapon.Attack(IsAttack, m_aniController);
                if (TryGetTarget(out RaycastHit hit, currentWeapon.m_maxDistance))
                {
                    _rangeWeapon.ApplyDamage(hit);
                }
            }
        }
        
            
    }
    /// <summary>
    /// 거리 내 맞은 대상이 있는지 확인 (단순 체크용)
    /// </summary>
    public bool TryGetTarget(out RaycastHit hit, float maxDistance)
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;

        bool isHit = Physics.Raycast(origin, dir, out hit, maxDistance, 1 << LayerMask.NameToLayer("Enemy"));

        return isHit;
    }

    public void SetColorMarkCrossHeadUI(bool isDistance)
    {
        m_uiManager.SetColorMarkCrossHead(isDistance);
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
        m_aniController.SetAnimatorWeight(1,1);
        m_aniController.SwapWeaponAni(CurrentWeaponNum, isFlying);

        if (CurrentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
        else m_uiManager.SetAmmo(0, 0, 0);
    }
    public void SwapInventoryWeapon()
    {
        m_swapAction?.Invoke(CurrentWeaponNum);
    }

    public void ExitSwapWeapon(bool isFlying)
    {
        if (CurrentWeaponNum > 1)
        {
            RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
            m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);
        }
        else m_uiManager.SetAmmo(0, 0, 0);

        if(!isFlying)
            m_aniController.SetAnimatorWeight(1, 0);
    }

    // TODO : 정리 후 고려
    /*public void AttackRootMotion(bool isApplyRoot)
    {
       // m_animationController.SetApplyRootMotion(isApplyRoot);
    }*/

    public void SetAming(bool isOffAiming = false)
    {
        m_isAiming = !m_isAiming;
        if (isOffAiming) m_isAiming = false;

        m_uiManager.ChangeSniperAimUI(m_isAiming? (CurrentWeaponNum == 3? true: false) : false);
        m_cameraManager.AimFOV(m_isAiming, CurrentWeaponNum == 3);
    }

    public bool EnterReload()
    {
        RangeWeapon _rangeWeapon = CurrentWeapon as RangeWeapon;
        bool _cansReload = _rangeWeapon.Reload();
        if(!_cansReload) return false;

        m_uiManager.SetAmmo(_rangeWeapon.CurrentAmmo, _rangeWeapon.SaveAmmo, _rangeWeapon.MaxAmmo);

        m_aniController.SetAnimatorWeight(1, 1);
        m_aniController.ReloadAni();
        m_audioManager.PlaySFXCombatAudio(SFXCombatType.Reload);
        return true;
    }
    public void ExitReload(bool isFlying)
    {
        if(!isFlying)
        m_aniController.SetAnimatorWeight(1, 0);
    }

    public void EnterSkill()
    {
        SkillKey = m_inputHandler.SkillKey;
        m_aniController.SkillAni(SkillKey);
        m_aniController.SetAnimatorWeight(2, 1);
    }
    public void ExitSkill()
    {
        m_aniController.SetAnimatorWeight(2, 0);
    }
    #endregion ================================================ /Enter,Exit State
}
