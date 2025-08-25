using System;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    private PlayerCore m_playerCore;
    private LocomotionGroundUtility m_groundUtility;
    private LocomotionFlyUtility m_flyUtility;
    private GravityUtility m_gravityUtility;

    [Header("[ Ref Component ]")]
    [SerializeField]
    private CharacterController m_characterController;
    [SerializeField]
    private Camera m_mainCamera;

    [Space(10)]

    [Header("[ HandleMove ]")]
    [SerializeField]
    private float m_walkSpeed = 2f;
    [SerializeField]
    private float m_sprintSpeed = 5f;
    [SerializeField]
    private float m_flySpeed = 4f;
    [SerializeField]
    private float m_flySprintSpeed = 8f;
    [SerializeField]
    private float m_speedLerpRate = 10f;
    private float m_currentSpeed;
    [Space(10)]

    [Header("[ HandleRotate ]")]
    private float m_rotationSmoothTime = 0.2f;

    [Header("[ Ground ]")]
    [SerializeField]
    private LayerMask m_groundMask;
    [SerializeField]
    private float m_groundDistance = 0.25f;
    public bool IsGrounded { get; private set; }

    [Header("[ Jump ]")]
    [SerializeField]
    private float m_jumpHeight = 5f;

    [Header("[ Gravity ]")]
    [SerializeField]
    private float m_baseGravity = -9.8f;
    [SerializeField]
    private float m_flyingGravity = -0.5f;
    [SerializeField]
    private float m_antiGravity = 1.5f;


    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;

    private Vector3 m_moveDirByCamera;
    
    private bool m_isJumpKeyDown;
    private float m_lastGroundTime;
    private bool m_isFlying;
    public bool IsFlyOff { get; private set; }
    
    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_groundUtility = new LocomotionGroundUtility();
        m_flyUtility = new LocomotionFlyUtility();
        m_gravityUtility = new GravityUtility();
    }

    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    #region ================================================================================ Movement
    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public Vector3 LocomotionGroundMovement()
    {
        Vector3 _moveDir = m_playerCore.InputHandler.MoveDir;
        bool _isSprint = m_playerCore.InputHandler.IsSprint;
        float _targetSpeed = _isSprint ? m_sprintSpeed : m_walkSpeed;

        HandleMove(_moveDir, _targetSpeed);
        HandleRotate();
        ApplyGravity();

        m_playerCore.AniController.SetGroundMoveAni(m_currentSpeed);
        
        return _moveDir;
    }

    // TODO : FlyMove와 리팩토링
    private void HandleMove(Vector3 moveDir, float targetSpeed)
    {
        m_currentSpeed = m_groundUtility.HandleMove(moveDir, targetSpeed, m_speedLerpRate, m_characterController);
        m_moveDirByCamera = m_groundUtility.GetMovieDir();
    }

    // FlyRotate와 리팩토링
    private void HandleRotate()
    {
        m_groundUtility.HandleRotate(this.gameObject, m_rotationSmoothTime);
    }
    #endregion ================================================================================ /Movement

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

        if (!m_isJumpKeyDown && IsGrounded && !m_isFlying)
        {
            m_velocity.y = -2f;
        }
        // Ground Anim Parameter
        m_playerCore.AniController.SetIsGround(IsGrounded);

        Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
    }
    #endregion ================================================================================ /Ground

    #region ================================================================================ Jump
    
    public void JumpStart()
    {
        m_isJumpKeyDown = true;
        IsGrounded = false;

        //등가속도운동 적용 (노션 참고)
        m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_baseGravity);   // m_jumpHeight = 점프 힘이기도함

        m_playerCore.AniController.SetJumpAni(m_isJumpKeyDown);
        m_playerCore.AniController.SetIsGround(IsGrounded);
    }
    public void JumpExit()
    {
        m_isJumpKeyDown = false;
        m_playerCore.AniController.SetJumpAni(m_isJumpKeyDown);
        m_playerCore.AniController.SetIsGround(IsGrounded);
    }

    #endregion ================================================================================ /Jump

    #region ================================================================================ Gravtiy
    // TODO : 중복 내용이기에 재사용성으로 전환
    public void ApplyGravity()
    {
        m_velocity = m_gravityUtility.ApplyGravity(m_baseGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
        CheckGround();
    }
    public void ApplyAntiGravity()
    {
        m_velocity = m_gravityUtility.ApplyAntiGravity(m_antiGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
        CheckGround();
    }
    public void ApplyFlyGravity()
    {
        m_velocity = m_gravityUtility.ApplyAntiGravity(m_flyingGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
    }
    #endregion ================================================================================ /Gravtiy

    #region ================================================================================ Fly

    public void FlyUpStart()
    {
        float _startDistance = 9f;  // 이동거리

        IsGrounded = false;
        m_isFlying = true;

        bool _isFlyUpStart = m_playerCore.InputHandler.IsFlyUp;
        // 등가속
        m_velocity.y = Mathf.Sqrt(_startDistance * 2f * m_antiGravity);

        m_playerCore.AniController.SetIsFly(m_isFlying, _isFlyUpStart);
    }
    public void FlyUp()
    {
        ApplyAntiGravity();
    }

    public void FlyUpExit()
    {
        m_playerCore.AniController.SetIsFly(m_isFlying, false);
    }

    /// <summary>
    /// FlyMove, FlyRotate, FlyMoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public Vector3 LocomotionFlyMovement()
    {
        Vector3 _moveDir = m_playerCore.InputHandler.MoveDir;
        bool _isSprint = m_playerCore.InputHandler.IsSprint;
        float _targetSpeed = _isSprint ? m_flySprintSpeed : m_flySpeed;
        bool _isFlyOff = m_playerCore.InputHandler.IsFlyOff;
        m_isFlying = !_isFlyOff;

        // Move & Rotate
        HandleFlyMove(_moveDir, _targetSpeed);
        HandleFlyRotate(_moveDir);

        // ApplyGravity
        //ApplyFlyGravity();

        // Ani
        m_playerCore.AniController.SetFlyMoveAni(_moveDir.x, _moveDir.z);
        
        return _moveDir;
    }

    // TODO : FlyMove와 리팩토링
    private void HandleFlyMove(Vector3 moveDir, float targetSpeed)
    {
        m_currentSpeed = m_groundUtility.HandleMove(moveDir, targetSpeed, m_speedLerpRate, m_characterController);
        
        m_moveDirByCamera = m_groundUtility.GetMovieDir();
    }
    // TODO : FlyMove와 리팩토링

    // FlyRotate와 리팩토링
    private void HandleFlyRotate(Vector3 moveDir)
    {
        m_flyUtility.HandleRotate(this.gameObject, m_mainCamera, moveDir, m_rotationSmoothTime);
    }

    public void FlyExit()
    {
        bool _isFlyUp = m_playerCore.InputHandler.IsFlyUp;
        m_playerCore.AniController.SetIsFly(m_isFlying, _isFlyUp);
    }
    #endregion ================================================================================ /Fly
}
