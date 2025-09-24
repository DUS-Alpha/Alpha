using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using Unity.Hierarchy;
using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    //[Header("[ Ref Component ]")]
    private PlayerInputHandler m_InputHandler;
    private PlayerAnimationController m_animationController;
    private CharacterController m_characterController;
    private LocomotionUtility m_locoUtility;

    [Header("[ HandleMove ]")]
    [SerializeField]
    private float m_baseSpeed = 3f;     // 기본적으로 달리게끔
    [SerializeField]
    private float m_aimSpeed = 1.5f;
    [SerializeField]
    private float m_flySpeed = 8f;
    [SerializeField]
    private float m_flyAimSpeed = 3f;
    [SerializeField]
    private float m_speedLerpRate = 10f;
    private float m_currentSpeed;
    [Space(10)]

    [Header("[ Ground ]")]
    [SerializeField]
    private LayerMask m_groundMask;
    [SerializeField]
    private float m_groundDistance = 0.25f;
    public bool IsGrounded { get; private set; }

    [Header("[ Jump ]")]
    [SerializeField]
    private float m_jumpHeight = 2f;

    [Header("[ Gravity ]")]
    public float BaseGravity { get; private set; } = -9.8f;
    [SerializeField]
    private float m_flyingGravity = -0.5f;
    [SerializeField]
    private float m_antiGravity = 5f;

    [Header("[ FlyUp ]")]
    [Tooltip("목표 높이"),SerializeField]
    private float m_targetFlyHeight = 10f;
    [Tooltip("초기 상승 속도"), SerializeField]
    private float m_initialFlySpeed = 15f;
    [Tooltip("감속 계수"), SerializeField]
    private float m_flyDecel = 6f;
    private float m_currentFlyHeight = 0f;      // 현재 FlyUp 높이

    // Combat 상태 제어를 위해

    public Vector3 Velocity => m_velocity;  //.y값 변경이 public으로는 이상하게 안됨 그래서 연결
    private Vector3 m_velocity;

    private Vector3 m_moveDirByCamera;
    
    private float m_lastGroundTime;

    private Vector3 m_airMove;   // Jump/Fall 시 XZ 이동 전용

    // ========================== Input
    public Vector3 MoveDir { get; private set; }
    private bool m_isAim;
    public bool IsJump { get; private set; }
    public bool IsDodge { get; private set; }
    public bool IsFlying { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
    public bool IsImmediatelyRot { get; private set; } // 캐릭터 회전 유무
    private void Awake()
    {
        m_locoUtility = new LocomotionUtility();
        m_characterController = GetComponent<CharacterController>();
    }

    public void InitializeModule(PlayerInputHandler inputHandler,PlayerAnimationController animationController)
    {
        m_InputHandler = inputHandler;
        m_animationController = animationController;
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }

    public void CheckInput()
    {
        MoveDir = m_InputHandler.MoveDir;
        IsFlyUp = m_InputHandler.IsFlyUp;
        IsFlyOff = m_InputHandler.IsFlyOff;
        IsDodge = m_InputHandler.IsDodge;
        m_isAim = m_InputHandler.IsAim;

        bool _isJump = m_InputHandler.IsJump;

        if (_isJump && IsGrounded)
        {
            IsJump = true;
        }

        if (IsFlyUp)
        {
            IsFlying = true;
        }
        else if (IsFlyOff)
        {
            IsFlying = false;
        }

        IsImmediatelyRot = m_isAim;

    }
    #region ================================================================================ Movement

    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public void Movement()
    {
        float _targetSpeed = IsFlying? (m_isAim ? m_flyAimSpeed : m_flySpeed) : m_baseSpeed;

        if (m_isAim)
        {
            m_animationController.DirMoveAni(MoveDir.x, MoveDir.z);
        }
        else
        {
            m_animationController.MoveAni(m_currentSpeed);
        }

        HandleRotate(MoveDir, m_isAim, IsFlying);
        HandleMove(MoveDir, _targetSpeed, IsFlying);
    }

    // TODO : FlyMove와 리팩토링
    private void HandleMove(Vector3 moveDir, float targetSpeed, bool isFlying)
    {
        m_currentSpeed = m_locoUtility.HandleMove(moveDir, targetSpeed, m_speedLerpRate, m_characterController, isFlying);
        m_moveDirByCamera = m_locoUtility.GetMovieDir();
    }

    // FlyRotate와 리팩토링
    private void HandleRotate(Vector3 moveDir, bool isAim, bool isFlying, bool isFlyUp = false)
    {
        m_locoUtility.HandleRotate(this.gameObject, moveDir, isAim, IsFlying, isFlyUp);
    }
    #endregion ================================================================================ /Movement
    private void Update()
    {
        CheckGround();
    }
    #region ================================================================================ Ground
    private void CheckGround()
    {
        // m_characterController.center 바닥에서 조금 띄어져있는 상태
        Vector3 _center = m_characterController.center;
        float _height = m_characterController.height;

        Vector3 _colliderButtomtr = transform.position + _center - (Vector3.up * (_height * 0.5f - m_characterController.skinWidth));

        bool _groundCheck = Physics.CheckSphere(_colliderButtomtr, m_groundDistance, m_groundMask);

        if (_groundCheck)
        {
            m_lastGroundTime = Time.time;
        }

        IsGrounded = (Time.time - m_lastGroundTime) <= 0.1f;

        if (IsGrounded && !IsJump && !IsFlying)
        {
            SetVelocityY(- 2f);
        }
            // Ground Anim Parameter
            m_animationController.SetIsGroundAni(IsGrounded);

        Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
    }
    #endregion ================================================================================ /Ground

    #region ================================================================================ Jump
    public void JumpStart()
    {
        IsGrounded = false;
        IsJump = true;
        //등가속도운동 적용 (노션 참고)
        SetVelocityY(Mathf.Sqrt(m_jumpHeight * -2f * BaseGravity));   // m_jumpHeight = 점프 힘이기도함

        m_airMove = m_moveDirByCamera.normalized * m_currentSpeed;

        m_animationController.JumpAni();
        m_animationController.SetIsGroundAni(IsGrounded);

        if (m_airMove != Vector3.zero)
            gameObject.transform.rotation = Quaternion.LookRotation(m_airMove);

        
    }
    public void JumpExit()
    {
        IsJump = false;
        //m_airMove = Vector3.zero;  // 공중 이동값 초기화
        m_animationController.SetIsGroundAni(IsGrounded);
    }
    // 점프 시 이동방향으로의 이동처리(Jump, Fall Update에 적용)
    public void AirMovement()
    {
        // 공중 이동 (XZ) + 중력 이동(Y)
        Vector3 move = m_airMove;   // XZ 고정값
        move.y = m_velocity.y;      // y축 중력

        // 점프일때는 바로 회전하지 않으면 다른 방향 바라본 상태로 점프가됨

        m_characterController.Move(move * Time.deltaTime);

    }
    #endregion ================================================================================ /Jump

    #region ================================================================================ Gravtiy
    // TODO : 중복 내용이기에 재사용성으로 전환
    public void ApplyGravity()
    {
        m_velocity = m_locoUtility.ApplyGravity(BaseGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
    }
    public void AntiGravity()
    {
        m_airMove = Vector3.zero;
        m_velocity = m_locoUtility.ApplyGravity(m_antiGravity, m_currentSpeed, Vector3.zero, m_velocity, m_characterController);
    }

    #endregion ================================================================================ /Gravtiy

    #region ================================================================================ Fly
    private float m_currentFlySpeed;
    public void FlyUpStart()
    {
        IsGrounded = false;

        m_currentFlyHeight = 0f;
        m_currentFlySpeed = m_initialFlySpeed;


        // Flying중 FlyUp시 회전되어 있던 캐릭터 상태를 곧게 세운후 위로 이동시키기 위해
        HandleRotate(Vector3.zero, false, true, true);

        // 등가속 (강하게 발사)
        //SetVelocityY(Mathf.Sqrt(2f * -BaseGravity * m_antiGravity)); // m_antiGravity는 높이, 조정 가능

        m_animationController.FlyAni(IsFlying, IsFlyUp);
    }
    public void FlyUpUpdate()
    {
        // 속도를 점점 줄임
        m_currentFlySpeed = Mathf.Max(0f, m_currentFlySpeed - m_flyDecel * Time.deltaTime);

        // 목표 높이까지 상승
        float deltaY = m_currentFlySpeed * Time.deltaTime;

        // 남은 높이보다 더 올라가면 조정
        if (m_currentFlyHeight + deltaY > m_targetFlyHeight)
            deltaY = m_targetFlyHeight - m_currentFlyHeight;

        m_currentFlyHeight += deltaY;

        // 수직 이동만 적용
        m_characterController.Move(Vector3.up * deltaY);
    }
    public void FlyUpExit()
    {
        m_animationController.FlyAni(IsFlying, false);
    }
    public void FlyingExit()
    {
        m_animationController.FlyAni(false, false);
    }
    public void SetVelocityY(float value)
    {
        m_velocity.y = value;
    }
    #endregion ================================================================================ /Fly
    public void EnterLanding()
    {
        
    }

    public void OnAnimatorMove()
    {
        // Combat 공격 시 RootMotion에 대한 애니메이션 포지션값은 모델만 움직이고 실제 오브젝트포지션은 변경안되기에 이를 오브젝트에 적용
        if (m_animationController.IsRootMotion)
        {
            m_animationController.UpdateAnimatorTransformValue();

            // Animator가 계산한 이동량을 가져와서 CharacterController에 적용
            Vector3 _deltaPosition = m_animationController.RootMotionPos;

            //_deltaPosition.y = m_playerCore.Locomotion.BaseGravity * Time.deltaTime; // 중력 보정 (필요 시)

            m_characterController.Move(_deltaPosition);
            m_characterController.transform.rotation *= m_animationController.RootMotionRot;
        }
    }
}
