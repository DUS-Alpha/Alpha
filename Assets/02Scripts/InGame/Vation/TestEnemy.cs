using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float m_hp;

    private float m_stayDamageDelay;

    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (m_hp > 0)
        {
            m_hp -= damageMassage.Damage;
            Debug.Log($"Enmey HP : {m_hp}");
        }

        if(m_hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_stayDamageDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            IDamageable _damageableTarget;
            if (other.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.Damage = 5;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);
                // 맞은 곳에 BloodEffect 재생(몬스터쪽에서해도 괜찮음)
                m_stayDamageDelay = 1;
                Debug.Log(other.name);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_stayDamageDelay += Time.deltaTime;

            if (m_stayDamageDelay < 1) return;

            IDamageable _damageableTarget;
            if (other.TryGetComponent<IDamageable>(out _damageableTarget))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.Damage = 5;

                // 데미지 전달
                _damageableTarget.ApplyDamage(_damageMassage);
                // 맞은 곳에 BloodEffect 재생(몬스터쪽에서해도 괜찮음)

                Debug.Log(other.name);
                m_stayDamageDelay = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_stayDamageDelay = 0;
        }
    }
}
