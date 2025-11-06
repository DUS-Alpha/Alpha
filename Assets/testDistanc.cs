using UnityEngine;

public class testDistanc : MonoBehaviour
{
   public Transform Target; //플레이어
   

    // Update is called once per frame
    void Update()
    {
        float speed = 2f; // 천천히 움직이고 싶다면 값 낮추기

        transform.position = Vector3.MoveTowards(
            transform.position,
            Target.position,
            speed * Time.deltaTime
        );
    }
}
