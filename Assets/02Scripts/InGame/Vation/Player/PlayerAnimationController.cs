using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


// 현재 애니메이터 레이는 전신을 사용하는 Base와 병렬로 사용되는 UpperBody레이어로 구성
// 전신에는 Combat 지상공격
// 그 외 원거리 공격 및 Fly하면서의 Melee 공격들은 UpperBody 병행처리
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    public Animator m_animator;
    public Vector3 RootMotionPos { get; private set; }
    public Quaternion RootMotionRot { get; private set; }
    public bool IsRootMotion => m_animator.applyRootMotion;
    public bool IsPlayAni {  get; private set; }
    private PlayerCombat m_combat;
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }
    private void Start()
    {
        SetAnimatorWeight(1, 0);
        SetAnimatorWeight(2, 0);
        SetAnimatorWeight(3, 0);
    }

    public void InitializeModule(PlayerCombat combat)
    {
        m_combat = combat;
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        //events.SwapWeaponAction += SwapWeaponAni;
    }
    public void SetAnimatorWeight(int index, float value)
    {
        if(index == -1)
        {
            m_animator.SetLayerWeight(0, value);
            SetAnimatorWeight(2, value);
            SetAnimatorWeight(3, value);
        }
        else
            m_animator.SetLayerWeight(index, value);
    }
    public float GetCurrentAniInfo(int index)
    {
        AnimatorClipInfo[] _clipInfos = m_animator.GetCurrentAnimatorClipInfo(index);
        AnimatorStateInfo _stateInfo = m_animator.GetCurrentAnimatorStateInfo(index);

        if (_clipInfos.Length > 0)
        {
            AnimationClip _currentClip = _clipInfos[0].clip;
            float _baseLength = _currentClip.length;
            float _actualLength = _baseLength / (m_animator.speed * _stateInfo.speedMultiplier);  // 실제 재생 속도반영한 길이

            return _actualLength;
        }
        return 0;
    }

    public bool GetIsMeleeAttackInfo(int index)
    {
        AnimatorStateInfo state = m_animator.GetCurrentAnimatorStateInfo(index);
        return state.IsName("Combo1") || state.IsName("Combo2") || state.IsName("Combo3") || state.IsName("Combo4");
    }

    #region ================================================================================ Locomotion

    public void MoveAni(float inputX, float inputY, bool isFly, bool isCombat)
    {
       if(isFly)
        {
            // 애니메이션 기울기 조절
            inputY = isCombat ? (inputY < -0.35f ? -0.35f : inputY) : inputY;
            inputY = isCombat ? (inputY > 0.25f ? 0.25f : inputY) : (inputY > 0.6f ? 0.6f : inputY);
            inputX = isCombat ? (inputX > 0.25f ? 0.25f : inputX) : inputX;
            inputX = isCombat ? (inputX < -0.25f ? -0.25f : inputX) : inputX;
        }

        m_animator.SetFloat("InputX", inputX, 0.1f, Time.deltaTime);
        m_animator.SetFloat("InputY", inputY, 0.1f, Time.deltaTime);
    }
    public void SetIsGroundAni(bool isGrounded)
    {
        m_animator.SetBool("IsGround", isGrounded);
    }

    public void JumpTriggerAni()
    {
        m_animator.Play("Jump");
    }
    public void DashTriggerAni()
    {
        m_animator.Play("Dash");
    }

    public void FlyUpTriggerAni()
    {
        SetFlyingAni(true);
        m_animator.Play("FlyUp");
    }
    public void SetFlyingAni(bool isFlying)
    {
        m_animator.SetBool("IsFlying", isFlying);
    }
    public void FlyFallAni()
    {
        m_animator.Play("Fall2");
    }

    public void HitTriggerAni()
    {
        m_animator.Play("Hit");
    }
    public void DieTriggerAni()
    {
        SetAnimatorWeight(-1, 0);
        m_animator.StopPlayback();
        m_animator.Play("Die");
    }
    #endregion ================================================================================ /Locomotion


    #region ================================================================================ CombatFlags
    
    public void SwapWeaponAni(int currentNum, bool isFlying)
    {
        m_animator.Play("SwapWeapon", 1);
        m_animator.SetInteger("WeaponNum", currentNum);

        if (isFlying) return;

        // Ground일 때
        switch (currentNum)
        {
            case 0:
                break;
            case 1:
                m_animator.Play("MeleeMoveTree");
                break;
            case 2:
            case 3:
                m_animator.Play("RangeMoveTree");
                break;
        }
    }
    public void SetIsInCombatAni(bool isInCombat)
    {
        m_animator.SetBool("IsInCombat", isInCombat);
    }
    public void MeleeComboTriggerAni(int num)
    {
        //m_animator.Play("Combo" + num, 2);
        m_animator.SetTrigger("MeleeCombo");
    }
    public void SetAttackAni(bool isAttack)
    {
        m_animator.SetBool("IsAttack", isAttack);
    }
    public void RangeShootingAni()
    {
        m_animator.Play("RangeShooting", 1);
    }
    public void ReloadAni()
    {
        m_animator.Play("Reload", 1);
    }
    public void SkillAni(string key)
    {
        m_animator.Play("Skill " + key.ToUpper(), 2);
    }
  
    public void UpdateAnimatorTransformValue()
    {
        if (m_animator.applyRootMotion)
        {
            RootMotionPos = m_animator.deltaPosition;
            RootMotionRot = m_animator.deltaRotation;
        }
    }
    #endregion ================================================================================ /Combat

}
