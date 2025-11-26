using UnityEngine;

public class DespawnObj : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        // 파티클이 더 이상 살아있지 않으면 비활성화
        if (ps != null && !ps.IsAlive(true))
        {
            PoolManager.Instance?.Despawn(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject.layer == 20) // 20번레이어는 현재 Enemy
            {
                return;
            }
        }
       
        if (other.CompareTag("Player"))
        {
            print("두번째 폭발 데미지");
            IDamageable _damageableTarget;
            if (other.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.damage = 15;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);

                Debug.Log(other.name);
            }
            
        }
    }
}
