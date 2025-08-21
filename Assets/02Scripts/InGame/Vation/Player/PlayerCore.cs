using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour
{
    [Header(" [ Ref Component ] ")]
    public PlayerInputHandler InputHandler;
    public PlayerAnimationController AniController;
    public PlayerStateMachine StateMachine;
    public PlayerLocomotion Locomotion;
    public PlayerCombat Combat;
    public bool isAction = false;
    private void Awake()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        AniController = GetComponentInChildren<PlayerAnimationController>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        StateMachine = GetComponent<PlayerStateMachine>();
        Initialize();
    }

    public void Initialize()
    {
        Locomotion.Initialize(this);
        Combat.Initialize();
        StateMachine.Initialize(this);
    }

    void Start()
    {
        SwitchState(new PlayerIdleState(this));
    }

    public void SwitchState(PlayerState playerState)
    {
        StateMachine.SwitchState(playerState);
    }

    /*private void MovementModules()
    {
        Vector3 _moveDir = InputHandler.MoveDir;
        bool _isSprint = InputHandler.IsSprint;
        bool _isFly = InputHandler.IsFly;
        float _currentMoveSpeed = Locomotion.m_currentSpeed;

        // Locomotion
        Locomotion.HandleMove(_moveDir, _isSprint);
        Locomotion.HandleRotate(_isFly);

        // Animation
        AniController.GroundMoveAni(_currentMoveSpeed);
    }*/
}
