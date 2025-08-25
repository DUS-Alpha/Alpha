using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class GravityUtility : MonoBehaviour
{
    public Vector3 ApplyGravity(float gravity, float currentSpeed, Vector3 moveDir, Vector3 velocity, CharacterController characterController)
    {
        // 점프 m_velocity적용 후 중력 적용
        velocity.y += gravity * Time.deltaTime;

        // 이동 방향과 중력
        Vector3 _gravityMove = (moveDir * currentSpeed) + velocity;
        characterController.Move(_gravityMove * Time.deltaTime);
        return velocity;
    }

    // 이름 가독성을 위해
    public Vector3 ApplyAntiGravity(float antiGravity, float currentSpeed, Vector3 moveDir, Vector3 velocity, CharacterController characterController)
    {
        return ApplyGravity(antiGravity, currentSpeed, moveDir, velocity, characterController);
    }

    public Vector3 ApplyFlyGravity(float flyGravity, float currentSpeed, Vector3 moveDir, Vector3 velocity, CharacterController characterController)
    {
        return ApplyGravity(flyGravity, currentSpeed, moveDir, velocity, characterController);
    }
}
