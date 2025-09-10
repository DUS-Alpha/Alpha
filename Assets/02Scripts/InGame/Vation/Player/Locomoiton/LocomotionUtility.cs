using UnityEngine;

public class LocomotionUtility
{
    // Move Config
    private float m_currentSpeed;

    // Rotate Config
    private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    // Dir Config
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;
    private Vector3 m_moveDirByCamera;

    #region ================================================================================ Movement
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="moveDir"></param>
    /// <param name="isAim"> true시 카메라 정면 방향으로 실시간 회전 </param>
    public void HandleRotate(GameObject gameObject, Vector3 moveDir, bool isAim)
    {
        Camera cam = Camera.main;

        //if (moveDir == Vector3.zero) return;

        Vector3 _dir;

        if (!isAim)
        {
            if (moveDir != Vector3.zero)
                _dir = m_moveDirByCamera; // 이동 입력 있을 때는 이동 방향
            else
                _dir = gameObject.transform.forward; // 입력 없으면 현재 바라보는 방향 유지
        }
        else
        {
            _dir = cam.transform.forward; // Aim 중에는 카메라 forward
        }

        _dir.y = 0f;
        Quaternion _targetRot;
        if (_dir != Vector3.zero)
            _targetRot = Quaternion.LookRotation(_dir);
        else
            _targetRot = Quaternion.identity;

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

    #endregion ================================================================================ /Movment

    #region ================================================================================ Gravity
    public Vector3 ApplyGravity(float gravity, float currentSpeed, Vector3 moveDir, Vector3 velocity, CharacterController characterController)
    {
        // 점프 m_velocity적용 후 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 이동 방향과 중력
        Vector3 _gravityMove = (moveDir * currentSpeed) + velocity;
        characterController.Move(_gravityMove * Time.deltaTime);
        return velocity;
    }
    #endregion ================================================================================ /Gravity
}
