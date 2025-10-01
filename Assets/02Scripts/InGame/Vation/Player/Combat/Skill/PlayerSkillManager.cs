using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public GameObject[] SkillEffests;

    public ParticleSystem Skill1;

    public void PlaySkill1()
    {
        Skill1.Play();
    }
}
