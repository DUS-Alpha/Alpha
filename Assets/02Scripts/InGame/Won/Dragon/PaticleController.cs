using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PaticleController : MonoBehaviour
{
    [SerializeField] float boomTime; // default 1 seconds; 터지는 시간
    [SerializeField] float destroyTime; // default 3 seconds; // 스킬이 끝나는 시간
    
    float decalValue; // 데칼이 점점 커지는 상황에 사용
    [SerializeField] GameObject decal; // 미리 표시하는 데칼
    [SerializeField] GameObject Particle; // 미리 표시하는 파티클
    [SerializeField] SphereCollider collider;
    
    Material decalMaterial;

    float currentTime;
    private void OnEnable()
    {
        currentTime = 0f;
        decalValue = 0f;
        decal.SetActive(true);

        var projector = decal.GetComponent<DecalProjector>();
        decalMaterial = new Material(projector.material);
        projector.material = decalMaterial;
        decalMaterial.SetFloat("_DecalRed", decalValue);;
        collider.enabled = false;
        
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime < boomTime)
        {
            print("데칼 진입");
            decalValue = Mathf.Lerp(0f, 0.5f, currentTime);
            decalValue = Math.Clamp(decalValue, 0f, 0.5f);
            decalMaterial.SetFloat("_DecalRed", decalValue);
        }
        if (currentTime > boomTime+0.1f && currentTime < destroyTime)
        {
            collider.enabled = true;
            decal.SetActive(false);
            Particle.SetActive(true);
           
        }
        
        if (currentTime > destroyTime)
        {
            Deactive();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable _damageableTarget;
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();

                _damageMassage.Damage = 20;

                
                _damageableTarget.ApplyDamage(_damageMassage);
            }
        }
    }

    void Deactive()
    {
        collider.enabled = false;
        StopAllCoroutines();
        Particle.SetActive(false);
        PoolManager.Instance.Despawn(gameObject);
    }
}
