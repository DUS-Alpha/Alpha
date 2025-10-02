using UnityEngine;

public class PlayerCombatEffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_attack1Particle;
    [SerializeField]
    private ParticleSystem m_attack2Particle;
    [SerializeField]
    private DamageRange m_attackDamageRange;


    [SerializeField]
    private ParticleSystem[] m_skill1;

    [SerializeField]
    private DamageRange[] m_skillDamageDamage;

    [SerializeField]
    private ParticleSystem[] m_skill2;

    public void PlayAttack1()
    {
        m_attack1Particle.Play();
        m_attackDamageRange.OnCollider();
    }
    public void PlayAttack2()
    {
        m_attack2Particle.Play();
        m_attackDamageRange.OnCollider();
    }
    public void OffAttackCollider()
    {
        m_attackDamageRange.OffCollider();
    }

   

    /// <summary>
    /// Animation KeyFrame에서 호출
    /// </summary>
    public void PlaySkill1()
    {
        for (int i = 0; i < m_skill1.Length; i++)
        {
            m_skill1[i].Play();
        }

        m_skillDamageDamage[0].OnCollider();
    }

    public void OffSkill1Collider()
    {
        m_skillDamageDamage[0].OffCollider();
    }

    public void PlaySkill2()
    {
        for (int i = 0; i < m_skill2.Length; i++)
        {
            m_skill2[i].Play();
        }

        m_skillDamageDamage[1].OnCollider();
    }
    public void OffSkill2Collider()
    {
        m_skillDamageDamage[1].OffCollider();
    }
}
