using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerLocomotion : MonoBehaviour
{
    private PlayerCore m_playerCore;
    private CharacterController m_characterController;
    private LocomotionUtility m_locoUtility;

    //[Header("[ Ref Component ]")]

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
    public float BaseGravity { get; private set; } = -9.8f;
    [SerializeField]
    private float m_flyingGravity = -0.5f;
    [SerializeField]
    private float m_antiGravity = 1.5f;

    public Vector3 Velocity => m_velocity;  //.y값 변경이 public으로는 이상하게 안됨 그래서 연결
    private Vector3 m_velocity;

    private Vector3 m_moveDirByCamera;
    
    private float m_lastGroundTime;

    public Vector3 MoveDir { get; private set; }
    public bool IsSprint { get; private set; }
    public bool IsJump { get; private set; }
    public bool CanFlyUp { get; private set; }
    public bool IsFlying { get; private set; }
    public bool IsFlyUp { get; private set; }
    public bool IsFlyOff { get; private set; }
   
    private void Awake()
    {
        m_locoUtility = new LocomotionUtility();
    }

    public void Initialize(PlayerCore playerCore)
    {
        m_playerCore = playerCore;
        m_characterController = m_playerCore.PlayerCharacterController;
    }
    private void Start()
    {
        m_playerCore.CheckInputAction += CheckInput;
    }

    public void CheckInput()
    {
        MoveDir = m_playerCore.InputHandler.MoveDir;
        IsSprint = m_playerCore.InputHandler.IsSprint;
        IsFlyUp = m_playerCore.InputHandler.IsFlyUp;
        IsFlyOff = m_playerCore.InputHandler.IsFlyOff;
        bool _isJump = m_playerCore.InputHandler.IsJump;

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
    }
    #region ================================================================================ Movement

    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public void Movement()
    {
        float _targetSpeed = IsSprint ? m_sprintSpeed : m_walkSpeed;

        HandleMove(MoveDir, _targetSpeed);
        HandleRotate(MoveDir, IsFlying);

        if (IsFlying)
        {
            m_playerCore.AniController.SetFlyMoveAni(MoveDir.x, MoveDir.z);
        }
        else
        {
            ApplyGravity();
            m_playerCore.AniController.SetGroundMoveAni(m_currentSpeed);
        }
    }

    // TODO : FlyMove와 리팩토링
    private void HandleMove(Vector3 moveDir, float targetSpeed)
    {
        m_currentSpeed = m_locoUtility.HandleMove(moveDir, targetSpeed, m_speedLerpRate, m_characterController);
        m_moveDirByCamera = m_locoUtility.GetMovieDir();
    }

    // FlyRotate와 리팩토링
    private void HandleRotate(Vector3 moveDir, bool isCurrentFlying)
    {
        m_locoUtility.HandleRotate(this.gameObject, moveDir, isCurrentFlying);
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

        if (!IsJump && IsGrounded && !IsFlying)
        {
            m_velocity.y = -2f;
        }

        // Ground Anim Parameter
        m_playerCore.AniController.SetIsGroundAni(IsGrounded);

        Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
    }
    #endregion ================================================================================ /Ground

    #region ================================================================================ Jump
    public void JumpStart()
    {
        IsGrounded = false;
        IsJump = true;
        //등가속도운동 적용 (노션 참고)
        m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * BaseGravity);   // m_jumpHeight = 점프 힘이기도함

        m_playerCore.AniController.SetJumpAni(IsJump);
        m_playerCore.AniController.SetIsGroundAni(IsGrounded);
    }
    public void JumpExit()
    {
        IsJump = false;
        m_playerCore.AniController.SetJumpAni(IsJump);
        m_playerCore.AniController.SetIsGroundAni(IsGrounded);
    }

    #endregion ================================================================================ /Jump

    #region ================================================================================ Gravtiy
    // TODO : 중복 내용이기에 재사용성으로 전환
    public void ApplyGravity()
    {
        if(IsFlyUp)
            m_velocity = m_locoUtility.ApplyGravity(m_antiGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
        else if(!IsFlying)
            m_velocity = m_locoUtility.ApplyGravity(BaseGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
        CheckGround();
    }
    #endregion ================================================================================ /Gravtiy

    #region ================================================================================ Fly
    public void FlyUpStart()
    {
        float _startDistance = 9f;  // 이동거리

        IsGrounded = false;
        IsFlying = true;

        bool _isFlyUpStart = m_playerCore.InputHandler.IsFlyUp;
        // 등가속
        m_velocity.y = Mathf.Sqrt(_startDistance * 2f * m_antiGravity);

        m_playerCore.AniController.SetIsFlyAni(IsFlying, _isFlyUpStart);
        StartCoroutine(FlyUpDelayCoroutine(0.4f));
    }

    // 애니메이션 모션 자연스럽게하기 위해
    private IEnumerator FlyUpDelayCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        CanFlyUp = true;
    }
    public void FlyExit()
    {
        CanFlyUp = false;
        m_playerCore.AniController.SetIsFlyAni(IsFlying, IsFlyUp);
    }
    #endregion ================================================================================ /Fly
}
