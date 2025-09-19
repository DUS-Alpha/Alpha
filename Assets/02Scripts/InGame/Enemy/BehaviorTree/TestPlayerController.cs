using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private CharacterController controller;
    private Vector3 moveDirection;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D 또는 ←, →
        float vertical = Input.GetAxis("Vertical");     // W, S 또는 ↑, ↓

        // 방향 벡터 계산
        moveDirection = new Vector3(horizontal, 0, vertical);

        // 카메라 방향 기준으로 이동하고 싶다면 다음 줄 사용
        // moveDirection = Camera.main.transform.TransformDirection(moveDirection);

        // 정규화로 대각선 속도 보정
        if (moveDirection.magnitude > 1f)
            moveDirection.Normalize();

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
