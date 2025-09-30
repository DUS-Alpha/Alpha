using System.Collections;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour, IDamageable
{
    //[Header("[ Ref Component ]")]
    private PlayerInputHandler m_InputHandler;
    private PlayerAnimationController m_animationController;
    private CharacterController m_characterController;
    private PlayerMovementUitility m_movementUtility;
    private PlayerCameraManger m_cameraManager;
    private AudioManager m_audioManager;

    [Header("[ Status ]")]
    [SerializeField]
    private PlayerStatus m_status;

    [Header("[ HandleMove ]")]
    [SerializeField]
    private float m_baseSpeed = 3f;     // 기본적으로 달리게끔
    [SerializeField]
    private float m_jumpSpeed = 2f;
    [SerializeField]
    private float m_combatSpeed = 1.5f;
    [SerializeField]
    private float m_flySpeed = 8f;
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
    [SerializeField]
    private float m_maxFlyingGauge;
    public float MaxFlyingGauge => m_maxFlyingGauge;
    private float m_flyingGauge;
    public float FlyingGauge=> m_flyingGauge;
    [SerializeField]
    private TextMeshProUGUI m_gaugeTMP;
    private float m_currentFlyHeight = 0f;      // 현재 FlyUp 높이


    // Combat 상태 제어를 위해
    public Vector3 Velocity => m_velocity;  //.y값 변경이 public으로는 이상하게 안됨 그래서 연결
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;
    private float m_lastGroundTime;
    private Vector3 m_lastMoveDir;   // Jump/Fall 시 XZ 이동 전용

    // ========================== Input
    public bool IsAction => m_isAction;
    private bool m_isAction;
    public Vector3 MoveDir { get; private set; }
    public bool IsJump { get; private set; }
    private bool m_isJumping;
    public bool IsFlyUp { get; private set; }
    public bool IsFlying => m_isFlying;
    private bool m_isFlying;
    public bool IsFlyOff { get; private set; }
    public bool IsDash {  get; private set; }
    public bool IsDodge { get; private set; }
    public bool IsDie;

    private void Awake()
    {
        m_movementUtility = new PlayerMovementUitility();
        m_characterController = GetComponent<CharacterController>();
        m_flyingGauge = m_maxFlyingGauge;
    }

    public void InitializeModule(PlayerInputHandler inputHandler,PlayerAnimationController animationController, PlayerCameraManger playerCameraManger, AudioManager audioManager)
    {
        m_InputHandler = inputHandler;
        m_animationController = animationController;
        m_cameraManager = playerCameraManger;
        m_audioManager = audioManager;
    }
    public void InitializeEvents(IPlayerEvents events)
    {
        events.CheckInputAction += CheckInput;
    }

    private void Start()
    {
        m_status.HP = 100;
        m_status.FlyGauge = 100;
    }

    public void InitializeLocotion()
    {
        MoveDir = Vector3.zero;
        IsFlyUp = false;
        IsDodge = false;
        IsJump = false;
        m_isJumping = false;
        m_isFlying = false;
        m_currentSpeed = 0;
    }

    public void CheckInput()
    {
        MoveDir = m_InputHandler.MoveDir;
        IsFlyUp = m_InputHandler.IsFlyUp;
        IsDodge = m_InputHandler.IsDodge;
        IsJump = m_InputHandler.IsJump;
    }

    public void SetIsAction(bool isAction)
    { 
        m_isAction = isAction; 
    }
    #region ================================================================================ Movement

    /// <summary>
    /// Move, Rotate, MoveAni 동시 처리
    /// </summary>
    /// <param name="payerCore"></param>
    /// <returns></returns>
    public void Movement(bool isCombating, bool isSniper)
    {
        float _targetSpeed;

        // Combat인가? 그리고 Flying상태인가?
        if (MoveDir.sqrMagnitude < 0.1f) _targetSpeed = 0;
        else
        {
            _targetSpeed = isCombating ? (m_isFlying ? m_flightCombatSpeed : m_combatSpeed) : (m_isFlying ?  m_flySpeed: m_baseSpeed);
        }

        HandleRotate(MoveDir, isCombating, isSniper);
        HandleMove(MoveDir, _targetSpeed, isCombating);
    }

    public void MoveEffect()
    {
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Foot);
    }
    public IEnumerator PlayFootSoundCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Foot);
    }

    private void HandleMove(Vector3 moveDir, float targetSpeed, bool isCombat)
    {
        m_currentSpeed = m_movementUtility.HandleMove(moveDir, targetSpeed, m_characterController, m_isFlying);
        m_moveDirByCamera = m_movementUtility.GetMovieDir();

        if (isCombat)
            m_animationController.CombatMoveAni(moveDir.x, moveDir.z);
        else
            m_animationController.MoveAni(m_currentSpeed);
    }

    // FlyRotate와 리팩토링
    private void HandleRotate(Vector3 moveDir, bool isCombat, bool isSniper)
    {
        Camera _camera = isSniper ? m_cameraManager.SniperCamera : m_cameraManager.MainCamera;
        m_movementUtility.HandleRotate(this.gameObject, moveDir, _camera, isCombat, m_isFlying);
    }
    public void AniKeyFrameMoveAudio()
    {
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Foot);
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

        if (IsGrounded && !m_isJumping && !m_isFlying)
        {
            SetVelocityY(- 2f);
            ChargingFlyingGauge();
        }
            // Ground Anim Parameter
        m_animationController.SetIsGroundAni(IsGrounded);
        if(m_flyingGauge >= m_maxFlyingGauge) m_flyingGauge = m_maxFlyingGauge;
        m_gaugeTMP.text = m_flyingGauge.ToString();
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

        m_lastMoveDir = m_moveDirByCamera.normalized * m_jumpSpeed;


        if (m_lastMoveDir != Vector3.zero)
            gameObject.transform.rotation = Quaternion.LookRotation(m_lastMoveDir);

        m_animationController.JumpAni();
        m_animationController.SetIsGroundAni(IsGrounded);
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Jump);
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
        Vector3 move = m_lastMoveDir;   // XZ 고정값
        move.y = m_velocity.y;      // y축 중력

        // 점프일때는 바로 회전하지 않으면 다른 방향 바라본 상태로 점프가됨

        m_characterController.Move(move * Time.deltaTime);
    }

    #endregion ================================================================================ /Jump

    #region ================================================================================ Gravtiy
    // TODO : 중복 내용이기에 재사용성으로 전환
    public void ApplyGravity()
    {
        m_velocity = m_movementUtility.ApplyGravity(BaseGravity, m_currentSpeed, m_moveDirByCamera, m_velocity, m_characterController);
    }
    public void AntiGravity()
    {
        m_lastMoveDir = Vector3.zero;
        m_velocity = m_movementUtility.ApplyGravity(m_antiGravity, m_currentSpeed, Vector3.zero, m_velocity, m_characterController);
    }

    #endregion ================================================================================ /Gravtiy

    #region ================================================================================ Fly
    private float m_flyUpSpeed;
    public void FlyUpStart()
    {
        m_isFlying = true;
        IsGrounded = false;

        m_animationController.SetAnimatorWeight(1,1);

        m_currentFlyHeight = 0f;
        m_flyUpSpeed = m_initialFlySpeed;

        MoveDir = Vector3.zero;
        // 똑바로 선 상태로 회전
        m_movementUtility.InitializeRotate(this.gameObject);

        // 등가속 (강하게 발사)
        //SetVelocityY(Mathf.Sqrt(2f * -BaseGravity * m_antiGravity)); // m_antiGravity는 높이, 조정 가능

        m_animationController.FlyUpAni();
        m_animationController.SetFlyingAni(m_isFlying);
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.FlyUp);
    }

    public void FlyUpUpdate()
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

    // Flying
    public void UpdateFlightMove()
    {
        // 이동이 없을 경우 사운드 끄기
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.FlyMove, MoveDir.sqrMagnitude < 0.1f);
 
        /*if (m_flyingGauge <= 0) m_flyingGauge = 0;
        else m_flyingGauge = m_flyingGauge - Time.deltaTime;*/
    }
    public void ChargingFlyingGauge()
    {
        if (m_flyingGauge <= m_maxFlyingGauge)
            m_flyingGauge += Time.deltaTime;
        else m_flyingGauge = m_maxFlyingGauge;
    }

    public void FlightMoveExit()
    {
        if(!IsDie) m_isFlying = false;
        m_animationController.SetFlyingAni(m_isFlying);
        MoveDir = Vector3.zero;

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
        m_audioManager.PlaySFXLocomotionAudio(SFXLomotionType.Land);
    }

    public void EnterDie()
    {
        m_animationController.DieAni();
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

        if(m_status.HP > 0)
        {
            m_status.HP -= damageMassage.damage;
            if(m_status.HP%2 == 0)
            {
                m_animationController.HitAni();
            }
           
        }
        else if (m_status.HP <= 0)
        {
            IsDie = true;
            m_status.HP = 0;
        }
        Debug.Log(m_status.HP);
    }
}
