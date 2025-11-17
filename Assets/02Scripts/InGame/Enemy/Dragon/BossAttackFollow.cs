using UnityEngine;

public class BossAttackFollow : MonoBehaviour
{
    

    [SerializeField]private Animator animator;

    // 애니메이션 시작 시 Root 기준 위치/회전
    private Vector3 initialRootLocalPos;
    private Quaternion initialRootLocalRot;

    void Start()
    {   
  
    }


}