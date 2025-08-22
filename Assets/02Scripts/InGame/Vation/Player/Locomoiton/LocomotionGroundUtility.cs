using UnityEngine;

public class LocomotionGroundUtility
{
    // Move Config
    private float m_currentSpeed;
    private float m_speedLerpRate = 10f;

    // Rotate Config
    private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    // Dir Config
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;
    public float HandleMove(Vector3 moveDir,float targetSpeed, CharacterController characterController)
    {
        Camera cam = Camera.main;
        float _targetSpeed = targetSpeed;
        if (moveDir.magnitude <= 0.1f)
        {
            _targetSpeed = 0;
        }
        
        m_currentSpeed = Mathf.Lerp(m_currentSpeed, _targetSpeed, Time.deltaTime * m_speedLerpRate);

        // TPS에서 이동 방식중 카메라 앞 기준 이동 방식
        Vector3 _forward = cam.transform.forward;
        _forward.y = 0f;
        Vector3 _right = cam.transform.right;

        m_moveDirByCamera = (_right * moveDir.x) + (_forward * moveDir.z);
        characterController.Move(m_moveDirByCamera * Time.deltaTime * m_currentSpeed);
        return m_currentSpeed;
    }

    // FlyRotate와 리팩토링
    public void HandleRotate(GameObject gameObject)
    {
        if (m_moveDirByCamera == Vector3.zero) return;

        Quaternion _targetRot = Quaternion.LookRotation(m_moveDirByCamera);

        Vector3 _targetEuler = _targetRot.eulerAngles;
        Vector3 _currentEuler = gameObject.transform.eulerAngles;

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
        gameObject.transform.rotation = Quaternion.Euler(0f, smoothY, 0f);

        // Fly 시 회전 값 계산 제대로하기
    }

}
