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
}
