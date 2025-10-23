using System;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2EventController : MonoBehaviour
{
    public Transform breathOrigin;   // 브레스 시작 위치
    public LayerMask hitMask;        // 감지할 레이어

    public float maxRange = 20f;
    public float radius = 3f;
    public float breathDamge = 10;
    
    // 💡 데미지 딜레이를 위한 변수 추가 
    [SerializeField]private const float DAMAGE_INTERVAL = 10f; // 데미지 적용 주기 (0.2초)
    private float _damageTimer = 0f;            // 타이머 변수
    

    private void FixedUpdate()
    {
        // 💡 타이머를 누적하고, 주기가 되면 DoDamageCheck를 호출
        _damageTimer += Time.fixedDeltaTime;
        
        // 0.5초(DAMAGE_INTERVAL)가 지났는지 확인
        if (_damageTimer >= DAMAGE_INTERVAL)
        {
            DoBreathDamage(); // 데미지 적용 로직 호출
            _damageTimer = 0f;  // 타이머 초기화
        }
    }

  

    // 💡 실시간 피격 판정 및 데미지 적용 (0.5초마다 호출됨)
    void DoBreathDamage()
    {
        Ray ray = new Ray(breathOrigin.position, breathOrigin.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, maxRange, hitMask);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                // 🎯 시각 디버그 (Scene 뷰)
                // 데미지가 들어가는 프레임에만 빨간색으로 그립니다.
                Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);
                Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.cyan, 0.5f);

                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable _damageableTarget))
                {
                    DamageMassage _damassage = new DamageMassage
                    {
                        //HitNormal = hit.normal,
                        //HitPoint = hit.point,
                        damage = breathDamge
                    };
                    // 💥 데미지 적용은 0.5초마다만 실행
                    print($"데미지 피격 : {breathDamge}");
                    _damageableTarget.ApplyDamage(_damassage);
                }
            }
        }
        else
        {
            // ❌ 아무것도 안 맞았을 때
            Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.gray, 0.5f);
            Debug.Log("⚠️ SphereCast: 아무것도 맞지 않았습니다.");
        }
    }
    
}