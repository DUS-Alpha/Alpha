using UnityEngine;

public class DespawnObj : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    void OnEnable()
    {
        // 자기 자신 포함해서 모든 자식 파티클 재생
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (var system in systems)
        {
            system.Clear();
            system.Play();
        }
    }

    void Update()
    {
        // 파티클이 더 이상 살아있지 않으면 비활성화
        if (ps != null && !ps.IsAlive(true))
        {
            PoolManager.Instance?.Despawn(gameObject);
        }
    }
}