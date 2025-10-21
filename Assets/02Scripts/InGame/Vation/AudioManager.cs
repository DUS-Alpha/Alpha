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

public enum SFX_UIType
{
    None,
    Open,
    Close,
}
[Serializable]
public struct SFX_UIMapping
{
    public SFX_UIType Type;
    public AudioClip AudioClip;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource[] m_bgmAudioSource;
    [SerializeField]
    private BGMMapping[] m_bgmMappings;
    private Dictionary<BGMType, AudioClip> m_bgmAudioDic;
    [Space(10)]


    [SerializeField]
    private AudioSource[] m_sfxUIAudioSource;
    [SerializeField]
    SFX_UIMapping[] m_sfxUIMappings;
    private Dictionary<SFX_UIType, AudioClip> m_sfxWorldAudioDic;
    [Space(10)]
    
    [SerializeField]
    private AudioSource m_sfxCombatAudio;
    [SerializeField]
    private AudioClip[] m_sfxCombatClips;

    private void Awake()
    {
        Instance = this;

        m_bgmAudioDic = new Dictionary<BGMType, AudioClip>();
        m_sfxWorldAudioDic = new Dictionary<SFX_UIType, AudioClip>();

        foreach (var map in m_bgmMappings)
        {
            if(m_bgmAudioDic.ContainsKey(map.Type))
            {
                m_bgmAudioDic.Add(map.Type, map.AudioClip);
            }
        }

        foreach (var map in m_sfxUIMappings)
        {
            if (m_sfxWorldAudioDic.ContainsKey(map.Type))
            {
                m_sfxWorldAudioDic.Add(map.Type, map.AudioClip);
            }
        }

    }
    private AudioClip GetBGMClip(BGMType type) => m_bgmAudioDic.TryGetValue(type, out AudioClip audioClip)? audioClip : null;
    private AudioClip GetSFXUIClip(SFX_UIType type) => m_sfxWorldAudioDic.TryGetValue(type, out AudioClip audioclip)? audioclip : null;


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
        m_sfxUIAudioSource[num].Stop();
    }

    // TODO 재활용성을 위해 PlaySFX로 통합할지 고민
    public void PlaySFXUIAudio(int num ,SFX_UIType sfxType, bool isPlayOneShot = false)
    {
        AudioClip _clip = GetSFXUIClip(sfxType);
        if (_clip == null) return;

        if (isPlayOneShot) m_sfxUIAudioSource[num].PlayOneShot(_clip);
        else
        {
            if(!m_sfxUIAudioSource[num].isPlaying)
            m_sfxUIAudioSource[num].clip = _clip;
            m_sfxUIAudioSource[num].Play();
        }
    }
}
