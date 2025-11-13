using alpha;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum BGMTypes
{
    None,
    Title,
    InGame,
    BossField1,
    BossField2
}

[Serializable]
public struct BGMMapping
{
    public BGMTypes Type;
    public AudioClip AudioClip;
}

public enum SFX_LoopTypes
{
    None,
    AirField
}
public enum SFX_UITypes
{
    None,
    Hover,
    Click,
    Open,
    Close,
}
public enum SFX_InteractionTypes
{
    None,
    ItemPickup,
    NPC
}

[Serializable]
public struct SFX_LoopMapping
{
    public SFX_LoopTypes Type;
    public AudioClip AudioClip;
}
[Serializable]
public struct SFX_UIMapping
{
    public SFX_UITypes Type;
    public AudioClip AudioClip;
}
[Serializable]
public struct SFX_InteractionMapping
{
    public SFX_InteractionTypes Type;
    public AudioClip AudioClip;
}


public class WorldAudioManager : MonoBehaviour
{
    public static WorldAudioManager Instance;

    #region ========== AudioMixer
    [Header("[ Audio Mix Config ]")]
    [SerializeField]
    private AudioMixer m_audioMixer;

    [Range(0, 1), SerializeField]
    private float m_volume_Master;
    [Range(0, 1), SerializeField]
    private float m_volume_BGM;
    [Range(0, 1),SerializeField]
    private float m_volume_SFX;
    public float GetVolume_Master => m_volume_Master;
    public float GetVolume_BGM => m_volume_BGM;
    public float GetVolume_SFX => m_volume_SFX;
    public float SetVolume_Master
    {
        set
        {
            m_volume_Master = Mathf.Clamp01(value);
            SettingVolume(EAudioMixerType.Master, m_volume_Master);
        }
    }

    public float SetVolume_BGM
    {
        set
        {
            m_volume_BGM = Mathf.Clamp01(value);
            SettingVolume(EAudioMixerType.BGM, m_volume_BGM);
        }
    }

    public float SetVolume_SFX
    {
        set
        {
            m_volume_SFX = Mathf.Clamp01(value);
            SettingVolume(EAudioMixerType.SFX, m_volume_SFX);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (m_audioMixer == null) return;

        SettingVolume(EAudioMixerType.Master, m_volume_Master);
        SettingVolume(EAudioMixerType.BGM, m_volume_BGM);
        SettingVolume(EAudioMixerType.SFX, m_volume_SFX);
    }
#endif

    #endregion ========== /AudioMixer


    #region ========== Mapping Settings
    [Header("[ Mapping Settings ]")]
    [Header("BGM")]
    [SerializeField] private AudioSource[] m_bgmAudioSource;
    [SerializeField] private BGMMapping[] m_bgmMappings;
    private Dictionary<BGMTypes, AudioClip> m_bgmAudioDic;

    [Header("SFX - UI")]
    [SerializeField] private AudioSource[] m_sfxUIAudioSource;
    [SerializeField] private SFX_UIMapping[] m_sfxUIMappings;
    private Dictionary<SFX_UITypes, AudioClip> m_sfxUIDic;

    [Header("SFX - Loop")]
    [SerializeField] private AudioSource[] m_sfxLoopAudioSource;
    [SerializeField] private SFX_LoopMapping[] m_sfxLoopMappings;
    private Dictionary<SFX_LoopTypes, AudioClip> m_sfxLoopDic;

    [Header("SFX - Interaction")]
    [SerializeField] private AudioSource[] m_sfxInteractionAudioSource;
    [SerializeField] private SFX_InteractionMapping[] m_sfxInteractionMappings;
    private Dictionary<SFX_InteractionTypes, AudioClip> m_sfxInteractionDic;

    [Header("SFX - Combat")]
    [SerializeField] private AudioSource m_sfxCombatAudio;
    [SerializeField] private AudioClip[] m_sfxCombatClips;
    #endregion ========== /Mapping Settings

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 🔹 Dictionary 초기화
        m_bgmAudioDic = new();
        m_sfxUIDic = new();
        m_sfxLoopDic = new();
        m_sfxInteractionDic = new();

        // 🔹 매핑 등록
        foreach (var map in m_bgmMappings)
        {
            if (!m_bgmAudioDic.ContainsKey(map.Type))
                m_bgmAudioDic.Add(map.Type, map.AudioClip);
        }

        foreach (var map in m_sfxUIMappings)
        {
            if (!m_sfxUIDic.ContainsKey(map.Type))
                m_sfxUIDic.Add(map.Type, map.AudioClip);
        }

        foreach (var map in m_sfxLoopMappings)
        {
            if (!m_sfxLoopDic.ContainsKey(map.Type))
                m_sfxLoopDic.Add(map.Type, map.AudioClip);
        }

        foreach (var map in m_sfxInteractionMappings)
        {
            if (!m_sfxInteractionDic.ContainsKey(map.Type))
                m_sfxInteractionDic.Add(map.Type, map.AudioClip);
        }
    }

    private void Start()
    {
        /*m_volume_Master = 1;
        m_volume_BGM = 1;
        m_volume_SFX = 1;*/
    }

    private void SettingVolume(EAudioMixerType audioMixerType, float volume)
    {
        if (volume <= 0.0001f)
            volume = 0.0001f; // log10(0)은 -무한대 방지 (0이하로 되었을 시 무한대가 되어 다시 소리 커짐)

        float _dB = Mathf.Log10(volume) * 20f;
        m_audioMixer.SetFloat(audioMixerType.ToString(), _dB);
    }

    // ====== 🔸 BGM ======
    private AudioClip GetBGMClip(BGMTypes type)
        => m_bgmAudioDic.TryGetValue(type, out var clip) ? clip : null;

    public void PlayBGMAudio(int index, BGMTypes type)
    {
        AudioClip clip = GetBGMClip(type);
        if (clip == null) return;

        m_bgmAudioSource[index].clip = clip;
        m_bgmAudioSource[index].loop = true;
        m_bgmAudioSource[index].Play();
    }

    public void StopBGM(int index)
    {
        if (index < m_bgmAudioSource.Length)
            m_bgmAudioSource[index].Stop();
    }

    // ====== 🔸 SFX (UI) ======
    private AudioClip GetSFXUIClip(SFX_UITypes type)
        => m_sfxUIDic.TryGetValue(type, out var clip) ? clip : null;

    public void PlaySFXUI(int index, SFX_UITypes type, bool isOneShot = false)
    {
        AudioClip clip = GetSFXUIClip(type);
        if (clip == null || index >= m_sfxUIAudioSource.Length) return;

        var src = m_sfxUIAudioSource[index];
        if (isOneShot) src.PlayOneShot(clip);
        else
        {
            if (!src.isPlaying)
            {
                src.clip = clip;
                src.Play();
            }
        }
    }

    public void PlayHover()
    {
        PlaySFXUI(0, SFX_UITypes.Hover, true);
    }

    public void PlayClick()
    {
        PlaySFXUI(0, SFX_UITypes.Click, true);
    }

    // ====== 🔸 SFX (Loop) ======
    private AudioClip GetSFXLoopClip(SFX_LoopTypes type)
        => m_sfxLoopDic.TryGetValue(type, out var clip) ? clip : null;

    public void PlaySFXLoop(int index, SFX_LoopTypes type, bool loop = true)
    {
        AudioClip clip = GetSFXLoopClip(type);
        if (clip == null || index >= m_sfxLoopAudioSource.Length) return;

        var src = m_sfxLoopAudioSource[index];
        src.clip = clip;
        src.loop = loop;
        src.Play();
    }

    public void StopSFXLoop(int index)
    {
        if (index < m_sfxLoopAudioSource.Length)
            m_sfxLoopAudioSource[index].Stop();
    }

    // ====== 🔸 SFX (Interaction) ======
    private AudioClip GetSFXInteractionClip(SFX_InteractionTypes type)
        => m_sfxInteractionDic.TryGetValue(type, out var clip) ? clip : null;

    public void PlaySFXInteraction(int index, SFX_InteractionTypes type, bool isOneShot = false)
    {
        AudioClip clip = GetSFXInteractionClip(type);
        if (clip == null || index >= m_sfxInteractionAudioSource.Length) return;

        var src = m_sfxInteractionAudioSource[index];
        if (isOneShot) src.PlayOneShot(clip);
        else
        {
            src.clip = clip;
            src.Play();
        }
    }

    // ====== 🔸 Combat SFX (단순 배열형) ======
    public void PlayCombatSFX(int index)
    {
        if (index < 0 || index >= m_sfxCombatClips.Length) return;
        m_sfxCombatAudio.PlayOneShot(m_sfxCombatClips[index]);
    }
}