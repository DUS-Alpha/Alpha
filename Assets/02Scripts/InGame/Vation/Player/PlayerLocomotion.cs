using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header(" [Ref Component] ")]
    [SerializeField]
    private CharacterController m_characterController;
    [SerializeField]
    private Camera m_mainCamera;
    [Space(10)]

    [Header(" [HandleMove Config] ")]
    [SerializeField]
    private float m_walkSpeed = 2f;
    [SerializeField]
    private float m_sprintSpeed = 5f;
    [SerializeField]
    private float m_speedLerpRate = 10f;
    [Space(10)]

    [Header(" [HandleRotate Config] ")]
    private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    private Vector3 m_moveDirByCamera;
    public float m_currentSpeed { get; private set; }

    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    public void Initialize()
    {

    }

    public void HandleMove(Vector3 moveDir, bool isSprint)
    {
        if (moveDir.magnitude <= 0.1f)
        {
            m_currentSpeed = 0;
            return;
        }
        float targetSpeed = isSprint ? m_sprintSpeed : m_walkSpeed;

        m_currentSpeed = Mathf.Lerp(m_currentSpeed, targetSpeed, Time.deltaTime * m_speedLerpRate);

        // TPS에서 이동 방식중 카메라 앞 기준 이동 방식
        Vector3 _forward = m_mainCamera.transform.forward;
        _forward.y = 0f;    // 이동 시 위를 보고 이동 키 누를시 y값으로 인해 위로 이동되기에 0처리
        Vector3 _right = m_mainCamera.transform.right;

        m_moveDirByCamera = (_right * moveDir.x) + (_forward * moveDir.z);
        m_characterController.Move(m_moveDirByCamera * Time.deltaTime * m_currentSpeed);
    }

    public void HandleRotate(bool isFly)
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
}
