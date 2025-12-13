using FIMSpace.Basics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;


namespace alpha
{
    // 현재 애니메이터 레이는 전신을 사용하는 Base와 병렬로 사용되는 UpperBody레이어로 구성
    // 전신에는 Combat 지상공격
    // 그 외 원거리 공격 및 Fly하면서의 Melee 공격들은 UpperBody 병행처리
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationManager : MonoBehaviour
    {
        private Animator m_animator;
        public Vector3 RootMotionPos { get; private set; }
        public Quaternion RootMotionRot { get; private set; }
        public bool IsRootMotion => m_animator.applyRootMotion;
        public bool IsPlayAni { get; private set; }

        private bool? m_prevCombat;
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }
        private void Start()
        {
            m_animator.SetLayerWeight(1, 0);
            m_animator.SetLayerWeight(2, 0);
            m_animator.SetLayerWeight(3, 0);
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
        public void SetMoveType(bool isCombat)
        {
            if (isCombat)
                m_animator.CrossFade("CombatMoveTree", 0.1f);
            else
                m_animator.CrossFade("BaseMoveTree", 0.1f);
        }

        public void MoveAni(float inputX, float inputY, bool isCombat)
        {
            if (m_prevCombat != isCombat)
            {
                SetMoveType(isCombat);

                m_prevCombat = isCombat;
            }

            m_animator.SetFloat("InputX", inputX, 0.05f, Time.deltaTime);
            m_animator.SetFloat("InputY", inputY, 0.05f, Time.deltaTime);   
        }

        public void SetFlightMoveType(bool isCombat)
        {
            if (isCombat)
                m_animator.CrossFade("CombatFlightMove", 0.1f);
            else
                m_animator.CrossFade("FlightMove", 0.1f);
        }
        public void FlightMoveAni(float inputX, float inputY, bool isCombat)
        {
            if (m_prevCombat != isCombat)
            {
                SetFlightMoveType(isCombat);
                m_prevCombat = isCombat;
            }

            // 애니메이션 기울기 조절
            inputY = isCombat ? (inputY < -0.35f ? -0.35f : inputY) : inputY;
            inputY = isCombat ? (inputY > 0.25f ? 0.25f : inputY) : (inputY > 0.6f ? 0.6f : inputY);
            inputX = isCombat ? (inputX > 0.25f ? 0.25f : inputX) : inputX;
            inputX = isCombat ? (inputX < -0.25f ? -0.25f : inputX) : inputX;

            m_animator.SetFloat("InputX", inputX, 0.05f, Time.deltaTime);
            m_animator.SetFloat("InputY", inputY, 0.05f, Time.deltaTime);
        }

        public void SetIsGroundParameter(bool isGrounded)
        {
            m_animator.SetBool("IsGround", isGrounded);
        }

        public void JumpAni()
        {
            m_animator.CrossFade("Jump", 0.1f);
        }
        public void FallAni(EFallType fallType)
        {
            switch(fallType)
            {
                case EFallType.NormalFall:
                    m_animator.CrossFade("Fall",0.2f);
                    break;
                case EFallType.FlyFall:
                    m_animator.CrossFade("Fall2", 0.1f);
                    break;
            }

            SetFlyingParameter(false);
        }
        public void LandAni(EFallType fallType)
        {
            switch (fallType)
            {
                case EFallType.NormalFall:
                    m_animator.CrossFade("Landing",0.143f,0,0.443f);
                    break;
                case EFallType.FlyFall:
                    m_animator.CrossFade("Landing2", 0.25f);
                    break;
            }
            
        }

        // ========== Fly
        public void SetFlyingParameter(bool isFlying)
        {
            m_animator.SetBool("IsFlying", isFlying);
        }
        public void FlyUpAni()
        {
            SetFlyingParameter(true);
            m_animator.CrossFade("FlyUp", 0.25f);
        }

        public void FlyingUpAni()
        {
            m_animator.CrossFade("FlyingUp", 0.25f);
        }
        public void HitAni()
        {
            m_animator.Play("Hit");
        }
        public void DieAni()
        {
            m_animator.StopPlayback();
            m_animator.Play("Die");
        }
        #endregion ================================================================================ /Locomotion

        public void DashAni()
        {
            m_animator.Play("Dash");
        }
        #region ================================================================================ CombatFlags

        public void SwapWeaponAni(int swapNum, bool isFlying)
        {
            m_animator.Play("Swap", 3);
            m_animator.SetInteger("SwapNum", swapNum);

            if (isFlying) return;

            // 레이어 싱크를 통해 무기별 관리
            switch (swapNum)
            {
                case 0:
                    break;
                case 1:
                    m_animator.SetLayerWeight(1,1);
                    m_animator.SetLayerWeight(2, 0);
                    break;
                case 2:
                case 3:
                    m_animator.SetLayerWeight(2, 1);
                    m_animator.SetLayerWeight(1, 0);
                    break;
                case 4:
                    break;
            }
        }
        public void SetAttackBtnParameter(bool isAttackBtn)
        {
            m_animator.SetBool("IsAttackBtn", isAttackBtn);
        }
        public void SetIsInCombatAni(bool isInCombat)
        {
            m_animator.SetBool("IsInCombat", isInCombat);
        }

        // ===== Melee
        public void MeleeComboAni(int num)
        {
            m_animator.Play("Combo" + num, 4);
        }

        // ===== Range
        public void RangeShootingAni()
        {
            m_animator.Play("RifleShooting", 3, 0f);
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
}