using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float m_hp;
    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (m_hp > 0)
        {
            m_hp -= damageMassage.damage;
            Debug.Log($"Enmey HP : {m_hp}");
        }

        if(m_hp <= 0)
        {
            Debug.Log("Die");
            Destroy(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
