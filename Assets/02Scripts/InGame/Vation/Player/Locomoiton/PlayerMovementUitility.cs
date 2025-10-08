using UnityEngine;

public class PlayerMovementUitility
{
    // Rotate Config
    private float m_rotationSmoothTime = 0.1f;
    private float m_currentSmoothVelocityX = 0;
    private float m_currentSmoothVelocityY = 0;
    private float m_currentSmoothVelocityZ = 0;

    // Dir Config
    public Vector3 Velocity => m_velocity;
    private Vector3 m_velocity;

    #region ================================================================================ Movement
    public Vector3 HandleMove(GameObject player, Vector3 moveDir, float targetSpeed, CharacterController characterController, bool isFlying)
    {
        Vector3 dir = player.transform.right * moveDir.x + player.transform.forward * moveDir.z;

        if (!isFlying)
            dir.y = 0f;


        if (isFlying)
        {
            // 위아래 이동 추가 (예: Space=Up, Ctrl=Down)
            dir += Vector3.up * moveDir.y;
        }
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        characterController.Move(dir * Time.deltaTime * targetSpeed);

        return dir;
    }

    public void HandleRotate(GameObject gameObject, Vector3 moveDir, Camera camera, bool isFlying)
    {
        // TODO : 카메라 방향이 아닌 일반 키입력방향에 대한 이동은 차후 키로 변경
        Camera cam = camera;

        Vector3 _dir= cam.transform.forward; // Aim 중에는 카메라 forward

        // 공중에서 이동 없을때는 공격중 허리만 위로 바라보게
        if(!isFlying || isFlying && moveDir.magnitude < 0.1f) _dir.y = 0f;

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
        // Fly 상태면 X/Y/Z 전체 적용
        gameObject.transform.rotation = isFlying ? Quaternion.Euler(smoothX, smoothY, smoothZ) : Quaternion.Euler(0f, smoothY, 0f);

    }

    // 똑바로 선상태
    public void InitializeRotate(GameObject gameObject)
    {
        Vector3 currentEuler = gameObject.transform.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(0f, currentEuler.y, 0f);
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
