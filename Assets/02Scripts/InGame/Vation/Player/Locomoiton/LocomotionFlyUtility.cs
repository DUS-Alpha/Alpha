using UnityEngine;

public class LocomotionFlyUtility
{
    // Move Config
    private float m_currentSpeed;

    // Rotate Config
    //private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    // Dir Config
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;
    public Vector3 GetMovieDir()
    {
        return m_moveDirByCamera;
    }

    public float HandleMove(Vector3 moveDir, float targetSpeed, float speedLerpRate, CharacterController characterController)
    {
        Camera cam = Camera.main;
        if (moveDir.magnitude <= 0.1f)
        {
            targetSpeed = 0;
        }

        m_currentSpeed = Mathf.Lerp(m_currentSpeed, targetSpeed, Time.deltaTime * speedLerpRate);    // m_speedLerpRate : 전환 시간

        // TPS에서 이동 방식중 카메라 앞 기준 이동 방식
        Vector3 _forward = cam.transform.forward;
        _forward.y = 0f;
        Vector3 _right = cam.transform.right;

        m_moveDirByCamera = (_right * moveDir.x) + (_forward * moveDir.z);
        characterController.Move(m_moveDirByCamera * Time.deltaTime * m_currentSpeed);
        return m_currentSpeed;
    }

    // FlyRotate와 리팩토링
    public void HandleRotate(GameObject gameObject, Camera mainCamera, Vector3 moveDir, float rotationSmoothTime)
    {
        if (moveDir == Vector3.zero) return;
        
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0f; // 지상에서는 수평만 유지
        forward.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(forward);
        Vector3 targetEuler = targetRot.eulerAngles;
        Vector3 currentEuler = gameObject.transform.eulerAngles;

        float smoothX = Mathf.SmoothDampAngle(
            currentEuler.x,
            targetEuler.x,
            ref m_currentSmoothVelocityX,
            rotationSmoothTime
        );

        float smoothY = Mathf.SmoothDampAngle(
            currentEuler.y,
            targetEuler.y,
            ref m_currentSmoothVelocityY,
            rotationSmoothTime
        );

        float smoothZ = Mathf.SmoothDampAngle(
            currentEuler.z,
            targetEuler.z,
            ref m_currentSmoothVelocityZ,
            rotationSmoothTime
        );
        
        gameObject.transform.rotation = Quaternion.Euler(0f, smoothY, 0f);
    }

}
