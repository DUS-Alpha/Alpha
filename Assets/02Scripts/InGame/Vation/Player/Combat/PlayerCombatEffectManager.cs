using System;
using UnityEngine;

[Serializable]
public struct SkillParticles
{
    public ParticleSystem[] SkillEffects;
}

public class PlayerCombatEffectManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] m_attackParticle;
    [SerializeField]
    private DamageRange m_attackDamageRange;


    [SerializeField]
    private SkillParticles[] m_skills;

    [SerializeField]
    private DamageRange[] m_skillDamageDamage;

    // Ani KeyFrame
    public void PlayAttack1()
    {
        m_attackParticle[0].Play();
        m_attackDamageRange.OnCollider();
    }
    public void PlayAttack2()
    {
        m_attackParticle[1].Play();
        m_attackDamageRange.OnCollider();
    }
    public void PlayAttack3()
    {
        m_attackParticle[2].Play();
        m_attackDamageRange.OnCollider();
    }
    public void PlayAttck4()
    {
        m_attackParticle[3].Play();
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
        for (int i = 0; i < m_skills[0].SkillEffects.Length; i++)
        {
            m_skills[0].SkillEffects[i].Play();
        }

        m_skillDamageDamage[0].OnCollider();
    }

    public void OffSkill1Collider()
    {
        m_skillDamageDamage[0].OffCollider();
    }

    public void PlaySkill2()
    {
        for (int i = 0; i < m_skills[1].SkillEffects.Length; i++)
        {
            m_skills[1].SkillEffects[i].Play();
        }

        m_skillDamageDamage[1].OnCollider();
    }
    public void OffSkill2Collider()
    {
        m_skillDamageDamage[1].OffCollider();
    }

    public void PlaySkill3()
    {
        for (int i = 0; i < m_skills[2].SkillEffects.Length; i++)
        {
            m_skills[2].SkillEffects[i].Play();
        }

        m_skillDamageDamage[2].OnCollider(true);
    }
    public void OffSkill3Collider()
    {
        m_skillDamageDamage[2].OffCollider();
    }
    public void PlaySkill4()
    {
        for (int i = 0; i < m_skills[3].SkillEffects.Length; i++)
        {
            m_skills[3].SkillEffects[i].Play();
        }

        m_skillDamageDamage[3].OnCollider();
    }
    public void OffSkill4Collider()
    {
        m_skillDamageDamage[3].OffCollider();
    }
}
