using alpha;
using System;
using UnityEngine;


namespace alpha
{
    [Serializable] 
    public struct MoveConfig
    {
        public float BaseSpeed;
        public float RunSpeed;
        public float CombatSpeed;
        public float CurrentSpeed;
    };
    [Serializable]
    public struct RotateConfig
    {
        public float RotateSpeed;
        public float TurnSmoothTime;
    }

    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotion : MonoBehaviour, IDamageable
    {
        //[Header("[ Ref Component ]")]
        private PlayerCore m_core;
        private PlayerAnimationManager m_aniM;
        private CharacterController m_characterCtrl;

        private PlayerGaugeManager m_playerStatsM;

        [Header("[ Ref Component ]")]
        [SerializeField]
        private PlayerAudioManager m_playerAudioController;
        [SerializeField]
        private EffectManager m_effectManager;

        public PlayerGroundController GroundController { get; private set; }
        public PlayerFlightController FlightController { get; private set; }

        [Header("[ Ground Config]")]
        [SerializeField] private MoveConfig m_groundMoveConfig;
        [SerializeField] private RotateConfig m_groundRotateConfig;
        [SerializeField] private LayerMask m_groundMask;
        [SerializeField] private float m_groundDistance = 0.25f;

        public GroundAndGravityUitility groundUitil { get; private set; } = new GroundAndGravityUitility();
        
        public bool IsGrounded { get; private set; }

        [Header("[ Fly Config]")]
        [SerializeField] private MoveConfig m_flyMoveConfig;
        [SerializeField] private RotateConfig m_floyRotateConfig;
        [Tooltip("목표 높이"), SerializeField] private float m_targetFlyHeight = 10f;
        [SerializeField] private float m_initialFlySpeed = 15f;
        [Tooltip("감속 계수"), SerializeField] private float m_flyDecel = 6f;
        private float m_currentFlyUpSpeed;
        private float m_currentFlyHeight = 0f;      // 현재 FlyUp 높이

        public float CurrentMoveSpeed { get; private set; }

        [Header("[ Jump ]")]
        [SerializeField] private float m_jumpHeight = 2f;

        [Header("[ Gravity ]")]
        public float BaseGravity { get; private set; } = -9.8f;
        [SerializeField] private float m_flyingGravity = -0.5f;
        [SerializeField] private float m_antiGravity = 5f;

        // Combat 상태 제어를 위해
        public Vector3 Velocity => m_velocity;  //.y값 변경이 public으로는 이상하게 안됨 그래서 연결
        private Vector3 m_velocity;
        private Vector3 m_lastMoveDir;   // Jump/Fall 시 XZ 이동 전용

        // ========================== Input
        // Lock이랑은 다른개념으로 해당 State가 되었을 때 동작중인 Combat을 중지시키고 NonCombat로 전환

        public bool IsRotLock { get; private set; }
        public bool IsJump { get; private set; }
        private bool m_isJumping;

        public bool IsFlying { get; private set; }
        public bool IsFlyingGaugeZero { get; private set; }
        public float ActionGauge { get; private set; }

        public bool IsDie;

        public bool IsUnCkeckGround { get; private set; }

        public bool IsLocomotionLock => m_isLocomotionLock;
        private bool m_isLocomotionLock;

        public event Action<float> OnDecreaseGauge;
        public event Action OnRegenrateGauge;
        public event Action OnResetTimer;

        private void Awake()
        {
            m_characterCtrl = GetComponent<CharacterController>();

            GroundController = new PlayerGroundController(m_characterCtrl);
            FlightController = new PlayerFlightController(m_characterCtrl);
        }

        public void InitializeModule(PlayerCore core)
        {
            m_core = core;
            m_aniM = m_core.AniManager;
            m_playerStatsM = core.GaugeManager;
        }

        public void OnUpdate()
        {
            if(!IsUnCkeckGround) 
                IsGrounded = groundUitil.GetIsGround(m_characterCtrl, m_groundMask);

            ActionGauge = m_playerStatsM.CurrentActionGauge;
        }

        public void SetLocomotionLock(bool isLocomotionLock)
        {
            m_isLocomotionLock = isLocomotionLock;
        }
        #region ================================================================================ Movement
        public void SetIsUnCheckGround(bool isUnCheck)
        {
            IsUnCkeckGround = isUnCheck;
        }
        /// <summary>
        /// Ground, Fly 공용 사용
        /// </summary>
        public void Movement(bool CanMove, Vector2 moveInput, Vector2 lookInput, bool isRun, bool isFly, bool isCombat)
        {
            if (!CanMove)
            {
                CurrentMoveSpeed = 0;
                return;
            }

            // 이동방식에 대한 전략패턴 적용
            IMovement _movement = !isFly? GroundController : FlightController;

            MoveConfig _moveConfig = !isFly ? m_groundMoveConfig : m_flyMoveConfig;
            RotateConfig _rotConfig = !isFly ? m_groundRotateConfig : m_floyRotateConfig;

            // 스피드 설정
            _moveConfig.CurrentSpeed = isRun ? _moveConfig.RunSpeed : _moveConfig.BaseSpeed;
            _moveConfig.CurrentSpeed = isCombat ? _moveConfig.CombatSpeed : _moveConfig.CurrentSpeed;

            CurrentMoveSpeed = _moveConfig.CurrentSpeed;
            
            // Movement 실행
            _movement.Rotate(lookInput, _rotConfig, this.gameObject);
            _movement.Move(moveInput, isRun, _moveConfig, this.gameObject);

            // 애니메이션
            m_aniM.MoveAni(moveInput.x, moveInput.y, isFly, isCombat);

            // 오디오
        }

        public void AniKeyFrameMoveAudio()
        {
            //m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Foot);
        }
        #endregion ================================================================================ /Movement

        #region ================================================================================ Jump
        public void JumpStart()
        {
            IsGrounded = false;
            m_isJumping = true;
            //등가속도운동 적용 (노션 참고)
            SetVelocityY(Mathf.Sqrt(m_jumpHeight * -2f * BaseGravity));   // m_jumpHeight = 점프 힘이기도함

            //m_lastMoveDir = m_lastMoveDir * m_jumpSpeed;
            /*if (m_lastMoveDir != Vector3.zero)
                gameObject.transform.rotation = Quaternion.LookRotation(m_lastMoveDir);*/

            m_aniM.JumpTriggerAni();
            m_aniM.SetIsGroundAni(IsGrounded);
            m_playerAudioController.PlayLocomotionAudio(0, SFX_LomotionType.Jump);
        }
        public void JumpExit()
        {
            m_isJumping = false;
            m_aniM.SetIsGroundAni(IsGrounded);
        }
        // 점프 시 이동방향으로의 이동처리(Jump, Fall Update에 적용)
        public void AirMovement()
        {
            // 공중 이동 (XZ) + 중력 이동(Y)
            Vector3 _move = m_lastMoveDir;   // XZ 고정값
            _move.y = m_velocity.y;      // y축 중력

            // 점프일때는 바로 회전하지 않으면 다른 방향 바라본 상태로 점프가됨

            m_characterCtrl.Move(_move * Time.deltaTime);
        }

        #endregion ================================================================================ /Jump
        #region ================================================================================ Dash
        public void DashEnter()
        {
            // 
            var _dashDir = m_lastMoveDir;

            // 게이지 타이머 초기화
            OnResetTimer?.Invoke();

            m_aniM.DashTriggerAni();

            gameObject.transform.rotation = Quaternion.LookRotation(_dashDir);

            m_effectManager.DashEffect();
            //m_isDashing = true;

            // 게이지 삭감
            OnDecreaseGauge?.Invoke(40);

            // Audio;
            m_playerAudioController.PlayLocomotionAudio(0, SFX_LomotionType.Dash, true);
        }

        public void UpdateDashMove()
        {
            Vector3 _move = m_lastMoveDir;

            m_characterCtrl.Move(_move * Time.deltaTime * 15f);
        }
        public void DashExit()
        {
            //m_isDashing = false;
            var _camDir = Camera.main.transform.forward;
            gameObject.transform.rotation = Quaternion.LookRotation(_camDir);
        }

        #endregion ================================================================================ /Dash

        #region ================================================================================ Fly
        

        public void EnterFlyUp(bool isWeapon)
        {
            m_currentFlyHeight = 0f;
            m_currentFlyUpSpeed = m_initialFlySpeed;

            m_aniM.FlyUpTriggerAni();

            m_playerAudioController.PlayLocomotionAudio(0, SFX_LomotionType.FlyUp);
        }

        public void UpdateFlyUp()
        {
            // 속도를 점점 줄임
            m_currentFlyUpSpeed = Mathf.Max(0f, m_currentFlyUpSpeed - m_flyDecel * Time.deltaTime);

            // 목표 높이까지 상승
            float deltaY = m_currentFlyUpSpeed * Time.deltaTime;

            // 남은 높이보다 더 올라가면 조정
            if (m_currentFlyHeight + deltaY > m_targetFlyHeight)
                deltaY = m_targetFlyHeight - m_currentFlyHeight;

            m_currentFlyHeight += deltaY;

            // 수직 이동만 적용
            m_characterCtrl.Move(Vector3.up * deltaY);
        }

        public void EnterFlightMove()
        {
            WorldAudioManager.Instance.PlaySFXLoop(0, SFX_LoopTypes.AirField);
        }
        // Flying
        public void UpdateFlightMove()
        {
            // 게이지 삭감
            OnDecreaseGauge?.Invoke(0.005f);
        }

        public void ExitFlightMove()
        {
            if (!IsDie) IsFlying = false;
            m_aniM.SetFlyingAni(IsFlying);
            m_aniM.FlyFallAni();

            // 똑바로 선 상태로 회전
            //m_movementUtility.InitializeRotate(this.gameObject);
        }
        public void SetVelocityY(float value)
        {
            m_velocity.y = value;
        }
        #endregion ================================================================================ /Fly
        public void EnterLanding()
        {
            //m_audioManager.StopSFXLoop(1);
            m_playerAudioController.PlayLocomotionAudio(0, SFX_LomotionType.Land);
            m_playerStatsM.ResetRegenerationActionTimer();
        }
        public void ExitLanding()
        {
            
        }
        public void EnterDie()
        {
            m_aniM.DieTriggerAni();

        }

        public void OnAnimatorMove()
        {
            // Combat 공격 시 RootMotion에 대한 애니메이션 포지션값은 모델만 움직이고 실제 오브젝트포지션은 변경안되기에 이를 오브젝트에 적용
            if (m_aniM.IsRootMotion)
            {
                m_aniM.UpdateAnimatorTransformValue();

                // Animator가 계산한 이동량을 가져와서 CharacterController에 적용
                Vector3 _deltaPosition = m_aniM.RootMotionPos;

                //_deltaPosition.y = m_playerCore.Locomotion.BaseGravity * Time.deltaTime; // 중력 보정 (필요 시)

                m_characterCtrl.Move(_deltaPosition);
                m_characterCtrl.transform.rotation *= m_aniM.RootMotionRot;
            }
        }

        public void ApplyDamage(DamageMassage damageMassage)
        {
            if (IsDie) return;

            /*if(m_status.HP > 0)
            {
                m_status.HP -= damageMassage.Damage;
                if(m_status.HP%2 == 0)
                {
                    m_animationController.HitTriggerAni();
                }

            }

            if (m_status.HP <= 0)
            {
                IsDie = true;
                m_status.HP = 0;
            }
            Debug.Log(m_status.HP);*/
        }
    }
}
