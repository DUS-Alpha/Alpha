using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour
{
    [Header(" [ Ref Component ] ")]
    [SerializeField]
    private PlayerInputHandler m_inputHandler;
    [SerializeField]
    private PlayerAnimationController m_aniController;
    [SerializeField]
    private PlayerStateMachine m_stateMachine;
    [SerializeField]
    private PlayerLocomotion m_locomotion;
    [SerializeField]
    private PlayerCombat m_combat;

    private void Awake()
    {
        m_inputHandler = GetComponent<PlayerInputHandler>();
        m_aniController = GetComponentInChildren<PlayerAnimationController>();
        m_stateMachine = GetComponent<PlayerStateMachine>();
        m_locomotion = GetComponent<PlayerLocomotion>();
        m_combat = GetComponent<PlayerCombat>();
        Initialize();
    }

    public void Initialize()
    {
        m_locomotion.Initialize();
        m_combat.Initialize();
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementModules();
    }

    private void MovementModules()
    {
        Vector3 _moveDir = m_inputHandler.MoveDir;
        bool _isSprint = m_inputHandler.IsSprint;
        bool _isFly = m_inputHandler.IsFly;
        float _currentMoveSpeed = m_locomotion.m_currentSpeed;
        
        // Locomotion
        m_locomotion.HandleMove(_moveDir, _isSprint);
        m_locomotion.HandleRotate(_isFly);

        // Animation
        m_aniController.GroundMoveAni(_currentMoveSpeed);
    }
    // 상태 전환은 외부 시스템이 필요할 수 있음 → public
    public void ChangeState(PlayerState newState)
    {
        m_stateMachine.ChangeState(newState);
    }
}
