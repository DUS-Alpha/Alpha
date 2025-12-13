using System;
using UnityEngine;
using UnityEngine.Events;

public class PatternStartZone : MonoBehaviour
{
    public string playerTag = "PlayerLocomotionManager"; //플레이어 태그 나중에 변경 될 수 있음
    public KeyCode startKey = KeyCode.G;
    
    public UnityEvent onStart; // 시작을 위한 이벤트 
    
    public bool inside = false; // 안쪽으로 들어왔는지 확인하는 변수

    [SerializeField] private GameObject DollyGroups;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inside = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inside = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(startKey))
        {
            DollyGroups.SetActive(true);
            onStart?.Invoke();
        }
    }
}
