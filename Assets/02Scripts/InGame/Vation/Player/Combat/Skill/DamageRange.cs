using UnityEngine;

public class DamageRange : MonoBehaviour
{
    [SerializeField]
    private Collider m_rangeCollider;
    public float damage;
    private void Start()
    {
        m_rangeCollider.enabled = false;
    }

    public void OnCollider()
    {
        m_rangeCollider.enabled = true;
    }
    public void OffCollider()
    {
        m_rangeCollider.enabled = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            IDamageable _damageableTarget;
            if (other.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.damage = damage;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);
            }
        }
    }
}
