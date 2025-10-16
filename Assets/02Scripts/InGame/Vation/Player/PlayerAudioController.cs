using FirstGearGames.SmoothCameraShaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SFX_LomotionType
{
    None,
    FootStep1,
    FootStep2,
    Jump,
    FlyUp,
    FlyMove,
    Land,
    Dash,
    FlyDash,
    Die,
}

public enum SFX_CombatType
{
    None,
    MeleeAttack,
    RangeRifltAttack,
    RangeSniperAttack,
    Reload,
    SwapWeapon,
    Dodge,
    Skill
}

[Serializable]
public struct SFX_LocomotionMapping
{
    public SFX_LomotionType Type;
    public AudioClip AudioClip;
}

[Serializable]
public struct SFX_CombatMapping
{
    public SFX_CombatType Type;
    public AudioClip AudioClip;
}



// 공통된 사운드 제외는 각 객체에서 사운드 관리(애니메이션 동작시에 사운드 삽입하기 위해)
public class PlayerAudioController : MonoBehaviour
{
    // 0 : main Audio, 1 : 타 사운드 중첩 필요시 사용
    [SerializeField]
    private AudioSource[] m_audioSources;

    [Header("[ Locomotion ]")]
    [SerializeField]
    private SFX_LocomotionMapping[] m_locomotionAudioMappings;
    private Dictionary<SFX_LomotionType, AudioClip> m_locomotionAudioDic;
    
    [Space(10)]


    [Header("[ Combat ]")]
    [SerializeField]
    private SFX_CombatMapping[] m_combatAudioTypes;

    [SerializeField]
    private AudioClip[] m_comboAttackClips;

    [SerializeField]
    private AudioClip[] m_skillClips;

    private void Awake()
    {
        m_locomotionAudioDic = new Dictionary<SFX_LomotionType, AudioClip>();

        foreach(var map in m_locomotionAudioMappings)
        {
            if(!m_locomotionAudioDic.ContainsKey(map.Type))
                m_locomotionAudioDic.Add(map.Type, map.AudioClip);
        }

        foreach (var map in m_locomotionAudioMappings)
        {
            if (!m_locomotionAudioDic.ContainsKey(map.Type))
                m_locomotionAudioDic.Add(map.Type, map.AudioClip);
        }
    }

    public AudioClip GetAudioClips(SFX_LomotionType type) => m_locomotionAudioDic.TryGetValue(type, out AudioClip clips) ? clips : null;

    public void FootStepAudio()
    {
       
    }

    public void PlayLocomotionAudio(int num, SFX_LomotionType lomotionAudioType, bool isPlayOneShot = false)
    {
        // TODO : m_audioSources[num].isPlaying진행중이라면 다른 오디오소스 재생되도록 활용
        AudioClip _audioClip = GetAudioClips(lomotionAudioType);
        if (_audioClip == null) return;

        if (isPlayOneShot)
            m_audioSources[num].PlayOneShot(_audioClip);
        else
        {
            if (!m_audioSources[num].isPlaying)
            {
                m_audioSources[num].clip = _audioClip;
                m_audioSources[num].Play();
            }
        }
        
    }
    public void StopCurrentAudio(int num)
    {
        m_audioSources[num].Stop();
    }

    public void PlayLandAudio()
    {

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
