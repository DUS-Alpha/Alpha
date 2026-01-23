using System;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Windows;

// Module : 실제 게임 행위를 실행하는 유일한 객체
namespace alpha
{
    
    [RequireComponent(typeof(CharacterController))]
    public class LocomotionModule : MonoBehaviour
    {
        // ==================== Ref Component
        [SerializeField] private CharacterController m_characterCtrl;
        private ILocomoAniPort m_aniViewPort;
        private IEffectPort m_effectPort;

        // ==================== Config Data : 고정적 데이터
        [Header("[ Speed Config ]")]
        [SerializeField] private float m_baseSpeed = 8f;
        [SerializeField] private float m_backMoveSpeed = 4f;
        [SerializeField] private float m_combatSpeed = 5f;

        // State Speed
        [SerializeField] private float m_dashSpeed = 15f;
        [SerializeField] private float m_jumpSpeed = 5f;
        [SerializeField] private float m_fallSpeed = 4f;
        [SerializeField] private float m_flyUpSpeed = 6f;

        [Header("[ Jump Config ]")]
        [SerializeField] private float m_jumpHeight = 5f;

        [Header("[ Rotate Config ]")]
        private float m_turnSmoothTime = 0.1f;

        [Header("[ Ground Config ]")]
        [SerializeField] private LayerMask m_groundMask;
        [SerializeField] private float m_groundDistance = 0.25f;

        [Header("[ Ground Config ]")]
        [SerializeField] private float m_gravity = -9.8f;

        [Header("[ FlyUp Config ]")]
        [SerializeField] private float m_flyUpHeight = 10f;

        // ==================== Runtime Data : 변하는 데이터

        public Vector2 MoveInput { get; set; }
        public bool IsMove => MoveInput != Vector2.zero;
        public bool IsRotLock { get; set; }
        public bool IsJump { get; set; }
        public bool IsDash { get; set; }
        public bool IsFly { get; set; }

        // StateData
        public LocomotionStateData StateData = new LocomotionStateData(); // TODO : 추후 필요한 변수들(넘겨줘야할) 저장
        public bool IsFlyUping { get; private set; }
        private bool m_isFlying;
        public EFallType m_currentFallType;

        public LocomotionRuntimeData RuntimeData = new LocomotionRuntimeData(); // TODO : 추후 필요한 변수들(넘겨줘야할) 저장
        // 이동
        private float m_currentSpeed;
        private Vector3 m_currentVelocity;

        // 회전
        private float m_currentSmoothVelocityX;
        private float m_currentSmoothVelocityY;
        private float m_currentSmoothVelocityZ;
        // 지상 체크
        public bool IsGrounded { get; private set; }
        private float m_lastGroundTime = 0;
        
        // 입력 저장 발행자 이벤트
        private IInputActionPort m_inputActionPort;
        private void Awake()
        {
            m_characterCtrl = GetComponent<CharacterController>();
        }

        #region ================== 입력 저장 ===================
        // PlayerCore의 Awake에서 바인딩
        public void BInd(IInputActionPort inputActionPort, ILocomoAniPort aniPort, IEffectPort effectPort)
        {
            m_inputActionPort = inputActionPort;
            m_aniViewPort = aniPort;
            m_effectPort = effectPort;
        }

        private void OnEnable()
        {
            // 구독 설정
            if (m_inputActionPort != null)
                m_inputActionPort.OnInputAction += OnInput;
        }

        private void OnDisable()
        {
            // 구독 해제
            if (m_inputActionPort != null)
                m_inputActionPort.OnInputAction -= OnInput;
        }

        // TODO : StateData로 갱신?
        public void OnInput(PlayerInputManager input)
        {
            MoveInput = input.MoveDirInput;
            IsRotLock = input.IsRotLock;
            IsJump = input.IsJump;
            IsDash = input.IsDash;
            IsFly = input.IsFlyUp;
        }
        #endregion ================= 입력 저장 ===================

        private void Update()
        {
            CheckGround();
            ApplyGravity();
            CharacterMovement();
        }
        public void CharacterMovement()
        {
            m_characterCtrl.Move(m_currentVelocity * Time.deltaTime * m_currentSpeed);
        }

        #region ================== 지상 이동 ===================
        public void SettingsGroundMove()
        {
            m_aniViewPort.SetMoveType(false);
        }

        public void GroundMovement(Vector2 moveInput, bool isRotateLock, bool isAttacking)
        {
            GroundRotate(isRotateLock);
            GroundMove(moveInput, isAttacking);
            // 애니메이션
            m_aniViewPort.MoveAni(moveInput.x, moveInput.y, isAttacking);
        }
        public void InitializeMove()
        {
            // 속력 변경
            m_currentSpeed = 0;

            // Y속력 초기화
            m_currentVelocity = Vector3.zero;

            // 이동 애니메이션 파라미터값 초기화(안하면 이전 이동값 가진 상태로 시작)
            m_aniViewPort.InitializeMoveAni();
        }

        private void GroundMove(Vector2 moveInput, bool isAttacking)
        {
            if (moveInput == Vector2.zero)
            {
                InitializeMove();
                return;
            }

            Vector3 _moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;
            
            if(_moveDir.magnitude > 1) _moveDir.Normalize();

            m_currentSpeed = moveInput.y > 0 ? m_baseSpeed : m_backMoveSpeed;
            m_currentSpeed = isAttacking ? m_combatSpeed : m_currentSpeed;

            // 세부 이동 데이터 저장
            m_currentVelocity.x = _moveDir.x;
            m_currentVelocity.z = _moveDir.z;
        }
        #endregion ================= 지상 이동 ===================

        #region ================== 점프 이동 ===================
        // 점프의 Update는 그냥 중력만 적용하면되기에 셋팅만 설정
        public void SettingsJumpMovement()
        {
            // 점프 방향으로 강제 회전
            ForceRotate(m_currentVelocity);

            // 속력 변경
            m_currentSpeed = m_jumpSpeed;

            // Y 속도 변경
            m_currentVelocity.y = m_jumpHeight;

            // 애니메이션
            m_aniViewPort.JumpAni();
        }

        public bool CheckedVelocityY()
        {
            return m_currentVelocity.y < 0;
        }
        #endregion ================== 점프 이동 ===================

        #region ================== 낙하 이동 ===================
        public void SettingsFallMovement()
        {
            // 낙하 방향으로 강제 회전
            m_currentVelocity.x = 0;
            m_currentVelocity.z = 0;

            ForceRotate(m_currentVelocity);

            // 속력 변경
            m_currentSpeed = m_fallSpeed;

            // 애니메이션
            m_aniViewPort.FallAni(m_currentFallType);

            // 부가 설정
            m_isFlying = false;
        }
        #endregion ================== 낙하 이동 ===================

        #region ================== 착지 ==================
        public void SettingsLand()
        {
            // 착지 애니메이션
            m_aniViewPort.LandAni(m_currentFallType);

            m_currentFallType = EFallType.NormalFall;
            
            InitializeMove();
        }

        #endregion ================ 착지 ==================
        // Land, Dash
        public float CheckedFinishAni()
        {
            return m_aniViewPort.GetCurrentAniInfo(0);
        }
        #region ================== 대시 이동 ===================
        public void SettingsDash()
        {
            ForceRotate(m_currentVelocity);

            m_currentSpeed = m_dashSpeed;
            
            m_aniViewPort.DashAni();

            m_effectPort.DashEffect();
        }

        public void DashMovement()
        {
            //m_currentVelocity.y += m_gravity * Time.deltaTime;
        }

        #endregion ================== 대시 이동 ===================
        #region ================== 수직 이동 ===================
        public void SettingsFlyUp()
        {
            m_currentFallType = EFallType.FlyFall;

            // 애니메이션
            m_aniViewPort.FlyUpAni();

            StartCoroutine(SetFlyUpCoroutine());
        }

        private IEnumerator SetFlyUpCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            // 속력 변경
            m_currentSpeed = m_flyUpSpeed;

            // 속도 변경
            m_currentVelocity.x = 0;
            m_currentVelocity.z = 0;
            m_currentVelocity.y = m_flyUpHeight;
        }
        #endregion ================== 수직 이동 ===================

        #region ================== 비행 이동 ===================
        public void SetFlightMovement()
        {
            m_isFlying = true;

            InitializeMove();

            m_aniViewPort.SetFlightMoveType(false);
        }

        public void AirMovement(Vector2 moveInput, bool isRotateLock, bool isCombat)
        {
            AirRotate(isRotateLock);
            FlightMove(moveInput, isCombat);

            // 애니메이션
            m_aniViewPort.FlightMoveAni(moveInput.x, moveInput.y, isCombat);
        }

        public void FlightMove(Vector2 moveInput, bool isCombat)
        {
            if (moveInput == Vector2.zero)
            {
                InitializeMove();
                return;
            }

            Vector3 _moveDir = transform.right * moveInput.x + transform.forward * moveInput.y;

            if (_moveDir.magnitude > 1) _moveDir.Normalize();

            m_currentSpeed = moveInput.y > 0 ? m_baseSpeed : m_backMoveSpeed;
            m_currentSpeed = isCombat ? m_combatSpeed : m_currentSpeed;

            // 세부 이동 데이터 저장
            m_currentVelocity.x = _moveDir.x;
            m_currentVelocity.y = _moveDir.y;
            m_currentVelocity.z = _moveDir.z;
        }

        #endregion ================== 비행 이동 ===================
        #region ================== 회전 ===================
        private void ForceRotate(Vector3 rotDir)
        {
            rotDir.y = 0f;
            if (rotDir.sqrMagnitude < 0.001f) return;

            transform.rotation = Quaternion.LookRotation(rotDir);
        }

        // 지상 회전
        private void GroundRotate(bool isRotateLock)
        {
            if(isRotateLock) return;
            float _targetRot = Camera.main.transform.eulerAngles.y; // 카메라 정면을 기준으로만 회전
            Vector3 _currentEuler = transform.eulerAngles;
            float smoothY = Mathf.SmoothDampAngle(_currentEuler.y, _targetRot, ref m_currentSmoothVelocityY, m_turnSmoothTime);
            transform.eulerAngles = Vector3.up * smoothY;
        }

        // 비행 회전
        private void AirRotate(bool isRotateLock)
        {
            if(isRotateLock) return;

            Vector3 _targetRot = Camera.main.transform.eulerAngles; // 카메라 정면을 기준으로만 회전

            Vector3 _currentEuler = transform.eulerAngles;

            float smoothX = Mathf.SmoothDampAngle(_currentEuler.x, _targetRot.x, ref m_currentSmoothVelocityX, m_turnSmoothTime);

            float smoothY = Mathf.SmoothDampAngle(_currentEuler.y, _targetRot.y, ref m_currentSmoothVelocityY, m_turnSmoothTime);

            float smoothZ = Mathf.SmoothDampAngle(_currentEuler.z, _targetRot.z, ref m_currentSmoothVelocityZ, m_turnSmoothTime);

            transform.rotation = Quaternion.Euler(smoothX, smoothY, smoothZ);
        }
        #endregion ================= 회전 ===================

        #region ================== 중력 ===================
        public void ApplyGravity()
        {
            if (m_isFlying) return;

            if (IsGrounded && m_currentVelocity.y < 0)
            {
                m_currentVelocity.y = -2f; // 바닥에 붙이기용
            }
            else
            {
                m_currentVelocity.y += m_gravity * Time.deltaTime;
            }
        }
        #endregion ================== 중력 ===================
        // 지상 체크
        public void CheckGround()
        {
            // m_characterController.center 바닥에서 조금 띄어져있는 상태
            Vector3 worldCenter = m_characterCtrl.transform.TransformPoint(m_characterCtrl.center);
            float _height = m_characterCtrl.height;

            Vector3 _colliderButtomtr = worldCenter - Vector3.up * (m_characterCtrl.height * 0.5f - m_characterCtrl.skinWidth);

            bool _groundCheck = Physics.CheckSphere(_colliderButtomtr, m_groundDistance, m_groundMask);

            // 점프같은 상태에서 바로 체크를 하면 True가 나온 후 False가 나오기에
            // 프레임 단위로 약간의 시간차를 두고 체크
            if (_groundCheck) m_lastGroundTime = Time.time;

            IsGrounded = (Time.time - m_lastGroundTime) <= 0.1f;
        }
    }
}