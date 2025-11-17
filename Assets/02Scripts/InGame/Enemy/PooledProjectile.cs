using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PooledProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;   // 몇 초 뒤 자동 반환
    
    [SerializeField] private GameObject hitEffectPrefab;

    private Rigidbody _rb;
    private float _deathTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 투사체 발사
    /// </summary>
    public void Launch(Vector3 dir, float speed, bool useGravity = false)
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();

        _rb.useGravity = useGravity;
        _rb.isKinematic = false;

        // 기존 속도 초기화
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // 새 속도 적용
        _rb.linearVelocity = dir.normalized * speed;

        _deathTime = Time.time + lifeTime;
    }


    private void Update()
    {
        if (Time.time >= _deathTime)
        {
            ReturnToPool();
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

        if (other.CompareTag("explosion"))
        {
            return;
        }
        
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        if (hitEffectPrefab != null)
        {
            PoolManager.Instance.Spawn(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        PoolManager.Instance?.Despawn(gameObject);
    }
    
  
}