using System;
using System.Collections.Generic;
using UnityEngine;




public enum BGMType
{
    None,
    AirField,
    BossField1,
    BossField2
}
[Serializable]
public struct BGMMapping
{
    public BGMType Type;
    public AudioClip AudioClip;
}

public enum SFX_WorldType
{
    None,
}
[Serializable]
public struct SFX_WorldMapping
{
    public SFX_WorldType Type;
    public AudioClip AudioClip;
}


public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] m_bgmAudioSource;
    [SerializeField]
    private BGMMapping[] m_bgmMappings;
    private Dictionary<BGMType, AudioClip> m_bgmAudioDic;
    [Space(10)]


    [SerializeField]
    private AudioSource[] m_sfxWorldAudioSource;
    [SerializeField]
    SFX_WorldMapping[] m_sfxWorldMappings;
    private Dictionary<SFX_WorldType, AudioClip> m_sfxWorldAudioDic;
    [Space(10)]
    
    [SerializeField]
    private AudioSource m_sfxCombatAudio;
    [SerializeField]
    private AudioClip[] m_sfxCombatClips;

    private void Awake()
    {
        m_bgmAudioDic = new Dictionary<BGMType, AudioClip>();
        m_sfxWorldAudioDic = new Dictionary<SFX_WorldType, AudioClip>();

        foreach (var map in m_bgmMappings)
        {
            if(m_bgmAudioDic.ContainsKey(map.Type))
            {
                m_bgmAudioDic.Add(map.Type, map.AudioClip);
            }
        }

        foreach (var map in m_sfxWorldMappings)
        {
            if (m_sfxWorldAudioDic.ContainsKey(map.Type))
            {
                m_sfxWorldAudioDic.Add(map.Type, map.AudioClip);
            }
        }

    }
    private AudioClip GetBGMClip(BGMType type) => m_bgmAudioDic.TryGetValue(type, out AudioClip audioClip)? audioClip : null;
    private AudioClip GetSFXWorldClip(SFX_WorldType type) => m_sfxWorldAudioDic.TryGetValue(type, out AudioClip audioclip)? audioclip : null;


    void Start()
    {
        m_bgmAudioSource[0].volume = 0.1f;
        m_bgmAudioSource[1].volume = 0.5f;
        m_bgmAudioSource[0].loop = true;
        m_bgmAudioSource[1].loop = true;

        PlayBGMAudio(0, BGMType.BossField2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayBGMAudio(int num, BGMType bgmType)
    {
        AudioClip _clip = GetBGMClip(bgmType);
        if (_clip == null) return;

        m_bgmAudioSource[num].clip = _clip;
        m_bgmAudioSource[num].Play();
    }
    public void StopSFX(int num)
    {
        m_sfxWorldAudioSource[num].Stop();
    }
    // TODO 재활용성을 위해 PlaySFX로 통합할지 고민
    public void PlaySFXLocomotionAudio(int num ,SFX_WorldType sfxType, bool isPlayOneShot = false)
    {
        AudioClip _clip = GetSFXWorldClip(sfxType);
        if (_clip == null) return;

        if (isPlayOneShot) m_sfxWorldAudioSource[num].PlayOneShot(_clip);
        else
        {
            if(!m_sfxWorldAudioSource[num].isPlaying)
            m_sfxWorldAudioSource[num].clip = _clip;
            m_sfxWorldAudioSource[num].Play();
        }
    }
    
    public void PlaySFXCombatAudio(SFX_CombatType sfxType, bool isStop = false, bool isPlayOneShot = false)
    {
        if (isStop) m_sfxCombatAudio.Stop();

        AudioClip _clip = m_sfxCombatClips[(int)sfxType - 1];

        m_sfxCombatAudio.clip = _clip;
        if (isPlayOneShot)
            m_sfxCombatAudio.PlayOneShot(_clip);
        else
        {
            if(!m_sfxCombatAudio.isPlaying)
            {
                m_sfxCombatAudio.Play();
            }
        }
    }

    // TODO : 사운드 페이드인아웃 효과 적용 필요
}
