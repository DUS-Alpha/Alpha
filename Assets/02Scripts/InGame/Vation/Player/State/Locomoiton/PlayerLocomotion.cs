using alpha;
using System.Collections;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour, IDamageable
{
    //[Header("[ Ref Component ]")]
    private PlayerInputManager m_InputHandler;
    private PlayerAnimationManager m_animationController;
    private CharacterController m_characterController;
    private PlayerMovementUitility m_movementUtility;
    private PlayerCameraManger m_cameraManager;
    private WorldAudioManager m_audioManager;
    private PlayerStatsManager m_playerStatsM;

    [Header("[ Ref Component ]")]
    [SerializeField]
    private PlayerAudioController m_playerAudioController;
    [SerializeField]
    private EffectManager m_effectManager;

    /*[Header("[ Status ]")]
    [SerializeField]
    private PlayerStatus m_status;*/

    [Header("[ HandleMove Speed]")]
    [SerializeField]
    private float m_baseSpeed = 3f;     // 기본적으로 달리게끔
    [SerializeField]
    private float m_baseBackMovingSpeed = 2f;
    [SerializeField]
    private float m_jumpSpeed = 2f;
    [SerializeField]
    private float m_flySpeed = 5f;
    [SerializeField]
    private float m_combatSpeed = 1.5f;
    [SerializeField]
    private float m_flightCombatSpeed = 1.5f;

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

    [Header("[ Fly ]")]
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
    private float m_lastGroundTime;
    private Vector3 m_lastMoveDir;   // Jump/Fall 시 XZ 이동 전용
    private Vector3 m_moveDir;

    // ========================== Input
    // Lock이랑은 다른개념으로 해당 State가 되었을 때 동작중인 Combat을 중지시키고 NonCombat로 전환
    public bool IsMove { get; private set; }
    public bool IsRotLock { get; private set; }
    public bool IsJump { get; private set; }
    private bool m_isJumping;
    public bool IsFlyUp { get; private set; }
    public bool IsFlying { get; private set; }
    public bool IsFlyFall { get; private set; }
    public bool IsFlyingGaugeZero { get; private set; }
    public float ActionGauge { get; private set; }

    public bool IsDash { get; private set; }
    private bool m_isDashing;
    public bool IsDie;
    private bool m_isUnCheckGround => IsFlying || m_isJumping || m_isDashing;

    public bool IsLocomotionLock => m_isLocomotionLock;
    private bool m_isLocomotionLock;
    
    private void Awake()
    {
        m_movementUtility = new PlayerMovementUitility();
        m_characterController = GetComponent<CharacterController>();
        m_playerStatsM = GetComponent<PlayerStatsManager>();
        m_InputHandler = GetComponent<PlayerInputManager>();
    }

    public void InitializeModule(PlayerAnimationManager animationController, PlayerCameraManger playerCameraManger, WorldAudioManager audioManager)
    {
        m_animationController = animationController;
        m_cameraManager = playerCameraManger;
        m_audioManager = audioManager;
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }
    public void InitializeLocotion()
    {
        m_moveDir = Vector2.zero;
        IsMove = false;
        IsRotLock = false;
        IsFlyUp = false;
        IsDash = false;
        IsJump = false;
        m_isJumping = false;
        IsFlying = false;
        m_currentSpeed = 0;
    }

    public void CheckInput()
    {
        IsRotLock = m_InputHandler.IsRotLock;
        if (m_isLocomotionLock) return;
        m_moveDir = m_InputHandler.MoveDirInput;
        IsMove = m_InputHandler.IsMove;
        IsFlyUp = m_InputHandler.IsFlyUp;
        IsDash = m_InputHandler.IsDash;
        IsJump = m_InputHandler.IsJump;
    }

    private void Start()
    {
        RealTimeUIManager.Instance.ActionGaugeUI(m_playerStatsM.SetMaxActionGauge(0)/100);    // 차후 레벨 표시
    }
    private void Update()
    {
        CheckGround();
        ActionGauge = m_playerStatsM.CurrentActionGauge;
        RealTimeUIManager.Instance.ActionGaugeUI(ActionGauge/100);
    }

    public void SetLocomotionLock(bool isLocomotionLock)
    {
        m_isLocomotionLock = isLocomotionLock;
    }
    #region ================================================================================ Movement
    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// 이동 정지 상태와 공격상태중의 이동에 대한 제한 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public void Movement(bool CanMove, bool isInCombat)
    {
        float _targetSpeed;
        float _speedLerpRate = 10f;

        // 1. 이동 스피드 계산
        if (!IsMove || !CanMove)
        {
            m_moveDir = Vector2.zero;
            _targetSpeed = 0;
        }
        else
        {
            if (m_moveDir.y > 0 && !isInCombat) _targetSpeed = IsFlying? m_flySpeed : m_baseSpeed;
            else _targetSpeed = isInCombat ? (IsFlying? m_flightCombatSpeed : m_combatSpeed) : m_baseBackMovingSpeed;
        }
        m_currentSpeed = Mathf.Lerp(m_currentSpeed, _targetSpeed, Time.deltaTime * _speedLerpRate);
        
        // 2. 이동 및 회전
        HandleMove(m_currentSpeed);
        
        // 
        if(!IsRotLock && CanMove)
            HandleRotate();
        
        // 3. 애니메이션
        m_animationController.MoveAni(m_moveDir.x, m_moveDir.y, IsFlying, CanMove);
    }

    private void HandleMove(float targetSpeed)
    {
        m_lastMoveDir = m_movementUtility.HandleMove(this.gameObject, m_moveDir, targetSpeed, m_characterController, IsFlying);
    }

    // FlyRotate와 리팩토링
    private void HandleRotate()
    {
        Camera _camera = m_cameraManager.MainCamera;
        m_movementUtility.HandleRotate(this.gameObject, m_moveDir, _camera, IsFlying);
    }

    public void AniKeyFrameMoveAudio()
    {
        //m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Foot);
    }
    #endregion ================================================================================ /Movement

    #region ================================================================================ Ground
    private void CheckGround()
    {
        // m_characterController.center 바닥에서 조금 띄어져있는 상태
        //Vector3 _center = m_characterController.center;
        Vector3 worldCenter = m_characterController.transform.TransformPoint(m_characterController.center);
        float _height = m_characterController.height;

        Vector3 _colliderButtomtr = worldCenter - Vector3.up * (m_characterController.height * 0.5f - m_characterController.skinWidth);

        bool _groundCheck = Physics.CheckSphere(_colliderButtomtr, m_groundDistance, m_groundMask);

        if (_groundCheck)
        {
            m_lastGroundTime = Time.time;
        }

        IsGrounded = (Time.time - m_lastGroundTime) <= 0.1f;

        if (IsGrounded && !m_isUnCheckGround)
        {
            SetVelocityY(- 2f);
            m_playerStatsM.RegenrateStamina();
        }
            // Ground Anim Parameter
        m_animationController.SetIsGroundAni(IsGrounded);

        //m_gaugeTMP.text = m_flyingGauge.ToString();
        Debug.DrawLine(_colliderButtomtr, _colliderButtomtr + (Vector3.down * m_groundDistance), Color.red);
    }
    #endregion ================================================================================ /Ground

    #region ================================================================================ Jump
    public void JumpStart()
    {
        IsGrounded = false;
        m_isJumping = true;
        //등가속도운동 적용 (노션 참고)
        SetVelocityY(Mathf.Sqrt(m_jumpHeight * -2f * BaseGravity));   // m_jumpHeight = 점프 힘이기도함

        //m_lastMoveDir = m_lastMoveDir * m_jumpSpeed;

        if (m_lastMoveDir != Vector3.zero)
            gameObject.transform.rotation = Quaternion.LookRotation(m_lastMoveDir);

        m_animationController.JumpTriggerAni();
        m_animationController.SetIsGroundAni(IsGrounded);
        m_playerAudioController.PlayLocomotionAudio(0,SFX_LomotionType.Jump);
    }
    public void JumpExit()
    {
        m_isJumping = false;
        m_animationController.SetIsGroundAni(IsGrounded);
    }
    // 점프 시 이동방향으로의 이동처리(Jump, Fall Update에 적용)
    public void AirMovement()
    {
        // 공중 이동 (XZ) + 중력 이동(Y)
        Vector3 _move = m_lastMoveDir;   // XZ 고정값
        _move.y = m_velocity.y;      // y축 중력

        // 점프일때는 바로 회전하지 않으면 다른 방향 바라본 상태로 점프가됨

        m_characterController.Move(_move * Time.deltaTime);
    }

    #endregion ================================================================================ /Jump
    #region ================================================================================ Dash
    public void DashEnter()
    {
        // 
        var _dashDir =m_lastMoveDir;

        m_playerStatsM.ResetRegenerationTimer();
        
        m_animationController.DashTriggerAni();

        gameObject.transform.rotation = Quaternion.LookRotation(_dashDir);
        
        m_effectManager.DashEffect();
        m_isDashing = true;

        m_playerStatsM.DecreaseActionGauge(40);
        // Audio;
        m_playerAudioController.PlayLocomotionAudio(0, SFX_LomotionType.Dash, true);
    }

    public void UpdateDashMove()
    {
        Vector3 _move = m_lastMoveDir;

        m_characterController.Move(_move * Time.deltaTime * 15f);
    }
    public void DashExit()
    {
        m_isDashing = false;
    }

    #endregion ================================================================================ /Dash


    #region ================================================================================ Gravtiy
    // TODO : 중복 내용이기에 재사용성으로 전환
    public void ApplyGravity()
    {
        m_velocity = m_movementUtility.ApplyGravity(BaseGravity, m_currentSpeed, m_lastMoveDir, m_velocity, m_characterController);
    }

    public void AntiGravity()
    {
        m_lastMoveDir = Vector3.zero;
        m_velocity = m_movementUtility.ApplyGravity(m_antiGravity, m_currentSpeed, Vector3.zero, m_velocity, m_characterController);
    }

    #endregion ================================================================================ /Gravtiy

    #region ================================================================================ Fly
    private float m_flyUpSpeed;
    // 충전
    
    // 감소
   

    public void EnterFlyUp(bool isWeapon)
    {
        IsGrounded = false;
        IsFlying = true;
        IsFlyFall = true;     // Jump, Fly Fall 구분
        m_currentFlyHeight = 0f;
        m_flyUpSpeed = m_initialFlySpeed;

        // 똑바로 선 상태로 회전 고친후 상승
        m_movementUtility.InitializeRotate(this.gameObject);

        m_animationController.FlyUpTriggerAni();

        m_playerAudioController.PlayLocomotionAudio(0,SFX_LomotionType.FlyUp);
        //m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.FlyUp);
    }

    public void UpdateFlyUp()
    {
        // 속도를 점점 줄임
        m_flyUpSpeed = Mathf.Max(0f, m_flyUpSpeed - m_flyDecel * Time.deltaTime);

        // 목표 높이까지 상승
        float deltaY = m_flyUpSpeed * Time.deltaTime;

        // 남은 높이보다 더 올라가면 조정
        if (m_currentFlyHeight + deltaY > m_targetFlyHeight)
            deltaY = m_targetFlyHeight - m_currentFlyHeight;

        m_currentFlyHeight += deltaY;

        // 수직 이동만 적용
        m_characterController.Move(Vector3.up * deltaY);
    }

    public void EnterFlightMove()
    {
        WorldAudioManager.Instance.PlaySFXLoop(0,SFX_LoopTypes.AirField);
    }
    // Flying
    public void UpdateFlightMove()
    {
        m_playerStatsM.DecreaseActionGauge(0.05f);
    }
    
    public void ExitFlightMove()
    {
        if(!IsDie) IsFlying = false;
        m_animationController.SetFlyingAni(IsFlying);
        m_animationController.FlyFallAni();

        // 똑바로 선 상태로 회전
        m_movementUtility.InitializeRotate(this.gameObject);
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
        m_playerStatsM.ResetRegenerationTimer();
    }
    public void ExitLanding()
    {
        IsFlyFall = false;
    }
    public void EnterDie()
    {
        m_animationController.DieTriggerAni();

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
