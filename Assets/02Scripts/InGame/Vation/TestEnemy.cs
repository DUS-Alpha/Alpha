using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    private float m_hp;
    public void ApplyDamage(DamageMassage damageMassage)
    {
        if (m_hp > 0)
        {
            m_hp -= damageMassage.damage;
        }
        Debug.Log($"Enmey HP : {m_hp}");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
