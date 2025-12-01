using System;
using UnityEngine;
using UnityEngine.Events;

public class ConditionReadyPos : MonoBehaviour
{
    public UnityEvent action;
    public string Enemy = "Enemy"; //플레이어 태그 나중에 변경 될 수 있음
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.CompareTag(Enemy))
        {
            print("닿았음");
            action?.Invoke();
        }
    }
}
