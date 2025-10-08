using UnityEngine;

// 공통된 사운드 제외는 각 객체에서 사운드 관리(애니메이션 동작시에 사운드 삽입하기 위해)
public class PlayerAudioPlay : MonoBehaviour
{
    // 0 : main Audio, 1 : 타 사운드 중첩 필요시 사용
    [SerializeField]
    private AudioSource[] m_audioSources;

    [SerializeField]
    private AudioClip[] m_footStepClips;

    public void FootStep1Audio()
    {
        m_audioSources[0].clip = m_footStepClips[0];
        //if (m_audioSources[0].isPlaying) return;
        m_audioSources[0].PlayOneShot(m_footStepClips[0]);
    }
    public void FootStep2Audio()
    {
        m_audioSources[0].clip = m_footStepClips[1];
        //if (m_audioSources[0].isPlaying) return;
        m_audioSources[0].PlayOneShot(m_footStepClips[1]);
    }
}
