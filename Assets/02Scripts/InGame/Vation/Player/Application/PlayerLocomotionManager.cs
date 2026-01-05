using System;
using UnityEngine;


namespace alpha
{
    [RequireComponent(typeof(GroundAndGravityUitility))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerLocomotionManager : MonoBehaviour, IDamageable
    {
        // Ref Module
        private PlayerCore m_core;
        private PlayerAnimationManager m_aniM;
        private PlayerGaugeManager m_playerStatsM;
        public CharacterController CharacterCtrl { get; private set; }
        private GroundAndGravityUitility m_groundAndGravityUitil;
        private EffectManager m_effectM;
        // 이동 관련 컨트롤러  
        private PlayerGroundController m_groundController;
        private PlayerFlightController m_flightController;

        [Header("[ Movement ]")]
        [SerializeField] private float m_baseSpeed = 8;
        [SerializeField] private float m_combatSpeed = 5;
        [SerializeField] private float m_turnSmoothTime = 0.1f;

        [Header("[ Gravity ]")]
        // Gravity
        [SerializeField] private float m_baseGravity = -15;
        [SerializeField] private float m_flyingGravity = -0.5f;
        [SerializeField] private float m_antiGravity = 5f;
        private GravityConfig m_gravityConfig;

        [Header("[ Ground ]")]
        [SerializeField] private LayerMask m_groundMask;
        public bool IsGrounded { get; private set; }
        private bool m_canCheckedGround = true;

        [Header("[ Jump ]")]
        [SerializeField] private float m_jumpHeight = 2.5f;
        public float JumpHeight => m_jumpHeight;

        [Header("[ Fly ]")]
        [SerializeField] private float m_targetFlyHeight = 10f;
        [SerializeField] private float m_flyDecel = 6f;             // 감속 계수
        [SerializeField] private float m_initialFlySpeed = 15f;     // 초기 상승 속도
        
        public float TargetFlyHeight => m_targetFlyHeight;
        public float FlyDecel => m_flyDecel;
        public float InitialFlySpeed => m_initialFlySpeed;

        // Public UsedValue
        public float CurrentSpeed { get; private set; }
        public Vector3 LastMoveInputDir { get; private set; } // Jump/Fall 시 XZ 이동 전용
        public EFallType CurrentFallType { get; private set; }
        public Vector3 CurrentVelocity => m_currentVelocity; // 프로퍼티 Vector 변수는 x,y,z 따로 설정안됨(get;set; 통일못한 이유)
        private Vector3 m_currentVelocity;

        public float ActionGauge { get; private set; }

        public event Action<float> OnDecreaseGauge;

        private void Awake()
        {
            CharacterCtrl = GetComponent<CharacterController>();

            m_gravityConfig = new GravityConfig()
            {
                BaseGravity = m_baseGravity,
                FlyingGravity = m_flyingGravity,
                AntiGravity = m_antiGravity
            };

            m_groundAndGravityUitil = new GroundAndGravityUitility(CharacterCtrl, m_gravityConfig);
            m_groundController = new PlayerGroundController(CharacterCtrl);
            m_flightController = new PlayerFlightController(CharacterCtrl);

        }

        public void InitializeModule(PlayerCore core)
        {
            m_core = core;
            /*m_aniM = m_core.AniManager;
            m_playerStatsM = core.GaugeManager;*/
        }

        public void OnUpdate()
        {
            CheckedGround();

            ActionGauge = m_playerStatsM.CurrentActionGauge;
        }

        #region ================================================================================ Ground Gravity
        // 지면일 시 속력 Y값을 -2로 고정하고 있기에 다른 Y값을 적용하기 위한 비활성화 필요 
        public void SetCheckedGround(bool isCheck)
        {
            m_canCheckedGround = isCheck;
        }

        // 지면 감지
        private void CheckedGround()
        {
            if (m_canCheckedGround)
            {
                m_groundAndGravityUitil.CheckedGround(m_groundMask);
                IsGrounded = m_groundAndGravityUitil.IsGround;
            }
            else IsGrounded = false;

            // 지면에 있을 시 Y속력 고정
            if (IsGrounded) m_currentVelocity.y = -2;

            // 애니메이션 파라미터 전달
            m_aniM.SetIsGroundParameter(IsGrounded);
        }

        public void SetCurrentVelocity(Vector3 velocity)
        {
            m_currentVelocity = velocity;
        }

        public void ApplyGravity()
        {
            float _CurrentGravity = m_groundAndGravityUitil.GetGravity();
            Vector3 _velocity = m_groundAndGravityUitil.ApplyGravity(_CurrentGravity,CurrentSpeed, LastMoveInputDir,CurrentVelocity);
            
            // 중력에 따른 Y속력 실시간 변환 (Fall에 대한 CurrentVelocity 값으로 전달)
            SetCurrentVelocity(_velocity);
        }
        #endregion ================================================================================ /Ground Gravity

        #region ================================================================================ Movement
        /// <summary>
        /// 키 입력 이동(Ground, Fly 공용 사용)
        /// X,Z 이동에 대한 내용만(Y속력 미포함)
        /// </summary>
        public void Movement(bool CanMove, Vector2 moveInput, Vector2 lookInput, bool isFly, bool isCombat)
        {
            if (!CanMove)
            {
                CurrentSpeed = 0;
                return;
            }

            // 이동방식에 대한 전략패턴 적용
            IMovement _movement = !isFly ? m_groundController : m_flightController;

            CurrentSpeed = moveInput.y > 0 ? m_baseSpeed : m_baseSpeed / 100 * 60;
            CurrentSpeed = isCombat ? m_combatSpeed : CurrentSpeed;

            // Movement 실행
            _movement.Rotate(lookInput, m_turnSmoothTime, this.gameObject);
            LastMoveInputDir = _movement.Move(moveInput, CurrentSpeed, this.gameObject);
        }

        /// <summary>
        /// 강제 수직 이동에 대한 값 설정(점프)
        /// </summary>
        public void SetForceVerticallMoveValue(float verticalHeight)
        {
            float _baseGravity = m_groundAndGravityUitil.GetGravity();
            //등가속도운동 적용
            m_currentVelocity.y = Mathf.Sqrt(verticalHeight * -2f * _baseGravity);
        }

        // 강제 이동 (Y속력 포함)
        public void ForceMove(Vector3 moveDir, float currentVelocityY)
        {
            // 공중 이동 (XZ) + 중력 이동(Y)
            Vector3 _move = moveDir;        // XZ 고정값
            _move.y = currentVelocityY;      // y축 중력

            CharacterCtrl.Move(_move * Time.deltaTime);
        }

        // 마지막 입력 이동방향으로 회전 (Jump, DashSkill)
        public void ForcedRotate(Vector3 dir)
        {
            gameObject.transform.rotation = Quaternion.LookRotation(dir);
        }
        #endregion ================================================================================ /Movement

        #region ================================================================================ ETC Set
        public void SetFallType(EFallType fallType)
        {
            CurrentFallType = fallType;
        }
        public void UpdateFlightMove()
        {
            // 게이지 삭감
            OnDecreaseGauge?.Invoke(0.005f);
        }

        #endregion ================================================================================ /ETC Set

        #region ================================================================================ Dash
        public void DashEnter()
        {
            // 
            var _dashDir = LastMoveInputDir;

            // 게이지 타이머 초기화
            //OnResetTimer?.Invoke();

            //m_aniM.DashAni();

            gameObject.transform.rotation = Quaternion.LookRotation(_dashDir);

            m_effectM.DashEffect();
            //m_isDashing = true;

            // 게이지 삭감
            OnDecreaseGauge?.Invoke(40);

            // Audio;
            //m_audioM.PlayLocomotionAudio(0, SFX_LomotionType.Dash, true);
        }

        public void UpdateDashMove()
        {
            Vector3 _move = LastMoveInputDir;

            CharacterCtrl.Move(_move * Time.deltaTime * 15f);
        }

        public void DashExit()
        {
            //m_isDashing = false;
            var _camDir = Camera.main.transform.forward;
            gameObject.transform.rotation = Quaternion.LookRotation(_camDir);
        }

        #endregion ================================================================================ /Dash

        public void OnAnimatorMove()
        {
            // Combat 공격 시 RootMotion에 대한 애니메이션 포지션값은 모델만 움직이고 실제 오브젝트포지션은 변경안되기에 이를 오브젝트에 적용
            if (m_aniM.IsRootMotion)
            {
                m_aniM.UpdateAnimatorTransformValue();

                // Animator가 계산한 이동량을 가져와서 CharacterController에 적용
                Vector3 _deltaPosition = m_aniM.RootMotionPos;

                //_deltaPosition.y = m_playerCore.Locomotion.BaseGravity * Time.deltaTime; // 중력 보정 (필요 시)

                CharacterCtrl.Move(_deltaPosition);
                CharacterCtrl.transform.rotation *= m_aniM.RootMotionRot;
            }
        }

        public void ApplyDamage(DamageMassage damageMassage)
        {
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
