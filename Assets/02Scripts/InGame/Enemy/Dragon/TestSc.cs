using UnityEngine;

public class TestSc : MonoBehaviour
{
    public Transform target;        // 목표 지점
    public float height = 3f;       // 포물선 높이
    public float duration = 1f;     // 이동에 걸리는 시간

    private Vector3 startPos;
    private float time;

    void Start()
    {
        transform.position = target.position;
    }

    void Update()
    {
   
    }
}
