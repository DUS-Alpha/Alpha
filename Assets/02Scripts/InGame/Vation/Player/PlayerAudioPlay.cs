using System.Collections;
using UnityEngine;

// 공통된 사운드 제외는 각 객체에서 사운드 관리(애니메이션 동작시에 사운드 삽입하기 위해)
public class PlayerAudioPlay : MonoBehaviour
{
    // 0 : main Audio, 1 : 타 사운드 중첩 필요시 사용
    [SerializeField]
    private AudioSource[] m_audioSources;

    [SerializeField]
    private AudioClip[] m_footStepClips;

    [SerializeField]
    private AudioClip[] m_comboAttackClips;

    [SerializeField]
    private AudioClip[] m_skillClips;
    public void FootStepAudio()
    {
        //if (moveSpeed < 0.1f ) return;
        //if (m_audioSources[0].isPlaying) return;
        int _random = Random.Range(0, m_footStepClips.Length);
        m_audioSources[0].clip = m_footStepClips[_random];
        m_audioSources[0].Play();

        m_audioSources[0].PlayOneShot(m_footStepClips[_random]);
    }

    private IEnumerator FootStepCoroutine()
    {
        yield return null;
    }

    public void PlayCombAttack1()
    {
        m_audioSources[1].clip = m_comboAttackClips[0];
        m_audioSources[1].Play();
    }
    public void PlayCombAttack2()
    {
        m_audioSources[1].clip = m_comboAttackClips[1];
        m_audioSources[1].Play();
    }
    public void PlayCombAttack3()
    {
        m_audioSources[1].clip = m_comboAttackClips[2];
        m_audioSources[1].Play();
    }
    public void PlayCombAttack4()
    {
        m_audioSources[1].clip = m_comboAttackClips[3];
        m_audioSources[1].Play();
    }

    public void PlaySkill1Audio()
    {
        m_audioSources[1].clip = m_skillClips[0];
        m_audioSources[1].Play();
    }
    public void PlaySkill2Audio()
    {
        m_audioSources[1].clip = m_skillClips[1];
        m_audioSources[1].Play();
    }
    public void PlaySkill3Audio()
    {
        m_audioSources[1].clip = m_skillClips[2];
        m_audioSources[1].Play();
    }
    public void PlaySkill4Audio()
    {
        m_audioSources[1].clip = m_skillClips[3];
        m_audioSources[1].Play();
    }
}
