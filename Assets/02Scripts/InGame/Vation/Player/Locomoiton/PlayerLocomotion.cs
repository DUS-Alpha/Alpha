using System;
using System.Collections;
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
    private LocomotionGroundUtility m_groundUtility;
    private LocomotionFlyUtility m_FlyUtility;

    [Space(10)]

    [Header("[ HandleMove Config ]")]
    [SerializeField]
    private float m_walkSpeed = 2f;
    [SerializeField]
    private float m_sprintSpeed = 5f;
    [SerializeField]
    private float m_flySpeed = 4f;
    [SerializeField]
    private float m_flySprintSpeed = 7f;

    [SerializeField]
    private float m_speedLerpRate = 10f;
    private float m_currentSpeed;
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

    public bool IsGrounded { get; private set; }
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;
    private PlayerCore m_playerCore;
    private bool m_isJumpKeyDown;
    private bool m_isFlyUpKey;
    private float m_lastGroundTime;
    private bool m_isFlyUpStart = false;
    public bool IsFlyDown { get; private set; }
    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_groundUtility = new LocomotionGroundUtility();
        m_FlyUtility = new LocomotionFlyUtility();
    }

    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
    }

    private void Start()
    {
        
    }
    #region ================================================================================ Movement
    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public Vector3 LocomotionGroundMovement()
    {
        CheckGround();

        Vector3 _moveDir = m_playerCore.InputHandler.MoveDir;
        bool _isSprint = m_playerCore.InputHandler.IsSprint;
        float _targetSpeed = _isSprint ? m_sprintSpeed : m_walkSpeed;

        HandleMove(_moveDir, _targetSpeed);
        HandleRotate();

        m_playerCore.AniController.SetGroundMoveAni(m_currentSpeed);
        return _moveDir;
    }

    // TODO : FlyMove와 리팩토링
    private void HandleMove(Vector3 moveDir, float targetSpeed)
    {
       m_currentSpeed = m_groundUtility.HandleMove(moveDir, targetSpeed, m_characterController);
    }

    // FlyRotate와 리팩토링
    private void HandleRotate()
    {
        m_groundUtility.HandleRotate(this.gameObject);
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

        if (!m_isJumpKeyDown && IsGrounded)
        {
            m_velocity.y = -2f;
        }

        // Ground Anim Parameter
        m_playerCore.AniController.SetIsGround(IsGrounded);
        
        Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
    }
    #endregion ================================================================================ /Ground

    #region ================================================================================ Jump
    public void ApplyGravity()
    {
        // 점프 m_velocity적용 후 중력 적용
        m_velocity.y += Physics.gravity.y * Time.deltaTime;

        // 이동 방향과 중력
        Vector3 _jumpMove = m_moveDirByCamera * (m_currentSpeed * 0.7f) + m_velocity;
        m_characterController.Move(_jumpMove * Time.deltaTime);

        CheckGround();
    }
    public void JumpStart()
    {
        m_isJumpKeyDown = true;
        IsGrounded = false;
        float _gravity = Physics.gravity.y;

        //등가속도운동 적용 (노션 참고)
        m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * _gravity);   // m_jumpHeight = 점프 힘이기도함
        
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

    #region ================================================================================ Fly
    public void ApplyAntiGravity()
    {
        float _acceleration = 1.5f; //가속도
        m_velocity.y += _acceleration * Time.deltaTime;

        // 이동 방향과 중력
        Vector3 _flyMove = m_moveDirByCamera + m_velocity;
        m_characterController.Move(_flyMove * Time.deltaTime);
    }
    public void FlyStart()
    {
        float _startDistance = 9f;  // 이동거리
        float _acceleration = 1.5f; //가속도

        // 등가속
        m_velocity.y = Mathf.Sqrt(_startDistance * 2f * _acceleration); 

        Vector3 _moveDir = m_playerCore.InputHandler.MoveDir;
        FlyAni(_moveDir, true, true);
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
        float _targetSpeed = _isSprint ? m_sprintSpeed : m_walkSpeed;
        bool _isFlyUp = m_playerCore.InputHandler.IsFlyUp;
        bool _isFlyDown = m_playerCore.InputHandler.IsFlyDown;

        if(_isFlyDown)
        {
            IsFlyDown = true;
        }
        else if(IsFlyDown && _isFlyUp)
        {
            IsFlyDown = false;
            FlyStart();
        }
        
        FlyAni(_moveDir, !_isFlyDown, _isFlyUp);

        if(IsFlyDown)
        {
            ApplyGravity();
            return _moveDir;
        }

        if (!_isFlyUp) m_isFlyUpStart = false;

        if (_isFlyUp && !m_isFlyUpStart)
        {
            StartCoroutine(EnableFlyUpAfterDelay(0.4f));
        }

        // 애니메이션에 맞추어 조금 늦게 올라가게
        else if (_isFlyUp && m_isFlyUpStart)
        {
            HandleFlyUp();
        }

        /*HandleFlyMove(_moveDir, _targetSpeed);
        HandleFlyRotate();*/

        return _moveDir;
    }
    private IEnumerator EnableFlyUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_isFlyUpStart = true;
    }
    private void HandleFlyUp()
    {
        ApplyAntiGravity();
    }
    // TODO : FlyMove와 리팩토링
    private void HandleFlyMove(Vector3 moveDir, float targetSpeed)
    {
        m_currentSpeed = m_FlyUtility.HandleFlyMove(moveDir, targetSpeed, m_characterController);
    }

    // FlyRotate와 리팩토링
    private void HandleFlyRotate()
    {
        m_FlyUtility.HandleRotate(this.gameObject);
    }

    public void FlyExit()
    {
        Vector3 _moveDir = m_playerCore.InputHandler.MoveDir;
        m_isFlyUpStart = false;

        FlyAni(_moveDir, false, false);
    }
    public void FlyAni(Vector3 moveDir, bool isFly,bool isFlyUp)
    {
        PlayerAnimationController _ani = m_playerCore.AniController;

        _ani.SetIsFly(isFly, isFlyUp);
        _ani.SetFlyMove(m_moveDirByCamera.x, m_moveDirByCamera.z);
    }

    #endregion ================================================================================ /Fly
}
