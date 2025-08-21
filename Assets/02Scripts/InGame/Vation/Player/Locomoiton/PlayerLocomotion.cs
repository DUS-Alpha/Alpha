using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("[ Ref Component ]")]
    [SerializeField]
    private CharacterController m_characterController;
    [SerializeField]
    private Camera m_mainCamera;
    [Space(10)]

    [Header("[ HandleMove Config ]")]
    [SerializeField]
    private float m_walkSpeed = 2f;
    [SerializeField]
    private float m_sprintSpeed = 5f;
    [SerializeField]
    private float m_speedLerpRate = 10f;
    [Space(10)]

    [Header("[ HandleRotate Config ]")]
    private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    [Header("[ Ground ]")]
    [SerializeField]
    private LayerMask m_groundMask;
    [SerializeField] 
    private float m_groundDistance = 0.25f;

    [Header("[ ApplyGravity ]")]
    [SerializeField]
    private float m_jumpHeight = 5f;

    public float CurrentSpeed { get; private set; }
    public bool IsGrounded { get; private set; }
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;
    private PlayerCore m_playerCore;
    private bool m_isJump;
    private float m_lastGroundTime;
    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    private void Update()
    {
        
    }

    #region ================================================================================ Movement

    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public Vector3 LocomotionMovement()
    {
        CheckGround();

        PlayerInputHandler _input = m_playerCore.InputHandler;
        PlayerAnimationController _aniController = m_playerCore.AniController;

        Vector3 _moveDir = _input.MoveDir;
        bool _isSprint = _input.IsSprint;
        bool _isFly = _input.IsFly;

        HandleMove(_moveDir, _isSprint);
        HandleRotate(_isFly);

        _aniController.SetGroundMoveAni(CurrentSpeed);
        return _moveDir;
    }

    private void HandleMove(Vector3 moveDir, bool isSprint)
    {
        if (moveDir.magnitude <= 0.1f)
        {
            CurrentSpeed = 0;
            return;
        }
        float targetSpeed = isSprint ? m_sprintSpeed : m_walkSpeed;

        CurrentSpeed = Mathf.Lerp(CurrentSpeed, targetSpeed, Time.deltaTime * m_speedLerpRate);

        // TPS에서 이동 방식중 카메라 앞 기준 이동 방식
        Vector3 _forward = m_mainCamera.transform.forward;
        _forward.y = 0f;    // 이동 시 위를 보고 이동 키 누를시 y값으로 인해 위로 이동되기에 0처리
        Vector3 _right = m_mainCamera.transform.right;

        m_moveDirByCamera = (_right * moveDir.x) + (_forward * moveDir.z);
        m_characterController.Move(m_moveDirByCamera * Time.deltaTime * CurrentSpeed);
    }

    private void HandleRotate(bool isFly)
    {
        if (m_moveDirByCamera == Vector3.zero) return;

        Quaternion _targetRot = Quaternion.LookRotation(m_moveDirByCamera);

        Vector3 _targetEuler = _targetRot.eulerAngles;
        Vector3 _currentEuler = transform.eulerAngles;

        // 각 축의 각도변화 Smooth 적용 (부드러운 회전)
        float smoothX = Mathf.SmoothDampAngle
                        (
                        _currentEuler.x, 
                        _targetEuler.x, 
                        ref m_currentSmoothVelocityX, 
                        m_rotationSmoothTime
                        );

        float smoothY = Mathf.SmoothDampAngle
                        (
                        _currentEuler.y, 
                        _targetEuler.y,
                        ref m_currentSmoothVelocityY, 
                        m_rotationSmoothTime
                        );

        float smoothZ = Mathf.SmoothDampAngle
                        (
                        _currentEuler.z, 
                        _targetEuler.z,
                        ref m_currentSmoothVelocityZ, 
                        m_rotationSmoothTime
                        );

        // 땅에서는 Y축의 각도만 사용하여 회전 적용, Fly상태에서는 전체 축 사용
        if(!isFly)
            transform.rotation = Quaternion.Euler(0f, smoothY, 0f);
        else
            transform.rotation = Quaternion.Euler(smoothX, smoothY, smoothZ);
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
        
        if(_groundCheck)
        {
            m_lastGroundTime = Time.time;
        }

        IsGrounded = (Time.time - m_lastGroundTime) <= 0.1f;

        if (!m_isJump && IsGrounded)
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
        m_isJump = true;
        IsGrounded = false;
        float _gravity = Physics.gravity.y;

        //등가속도운동 적용
        m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * _gravity);   // m_jumpHeight = 점프 힘이기도함
        
        m_playerCore.AniController.JumpAni(m_isJump);
        m_playerCore.AniController.SetIsGround(IsGrounded);
    }
    public void JumpExit()
    {
        m_isJump = false;
        m_playerCore.AniController.JumpAni(m_isJump);
        m_playerCore.AniController.SetIsGround(IsGrounded);
    }
    public void ApplyGravity()
    {
        // 점프 m_velocity적용 후 중력 적용
        m_velocity.y += Physics.gravity.y * Time.deltaTime;
        
        // 이동 방향과 중력
        Vector3 _jumpMove = m_moveDirByCamera * (CurrentSpeed * 0.7f) + m_velocity;
        m_characterController.Move(_jumpMove * Time.deltaTime);

        CheckGround();
    }
    #endregion ================================================================================ /Jump
}
