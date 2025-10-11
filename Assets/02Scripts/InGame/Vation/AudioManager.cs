using UnityEngine;

public enum SFXLomotionType
{
    None,
    Foot,
    Jump,
    FlyUp,
    FlyMove,
    Land,
    Die,
}
public enum SFXCombatType
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

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_bgmAudio;
    [SerializeField]
    private AudioClip[] m_bgmClips;
    [Space(10)]

    [SerializeField]
    private AudioSource m_sfxLocomotionAudio;
    [SerializeField]
    private AudioClip[] m_sfxLocomotionClips;
    [Space(10)]
    
    [SerializeField]
    private AudioSource m_sfxCombatAudio;
    [SerializeField]
    private AudioClip[] m_sfxCombatClips;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBGMAudio()
    {

    }
    public void SetSFXLocomotionAudioLoop(bool isLoop)
    {
        m_sfxLocomotionAudio.loop = isLoop;
    }
    // TODO 재활용성을 위해 PlaySFX로 통합할지 고민
    public void PlaySFXLocomotionAudio(SFXLomotionType sfxType, bool isStop = false, bool isPlayOneShot = false)
    {
        if (isStop)
        {
            m_sfxLocomotionAudio.Stop();
            return;
        }
        
        AudioClip _clip = m_sfxLocomotionClips[(int)sfxType-1];

        if(m_sfxLocomotionAudio.clip != _clip) m_sfxLocomotionAudio.Stop();

        m_sfxLocomotionAudio.clip = _clip;
        if (isPlayOneShot)
        m_sfxLocomotionAudio.PlayOneShot(_clip);
        else
        {
            if(!m_sfxLocomotionAudio.isPlaying)
            {
                m_sfxLocomotionAudio.Play();
            }
        }
    }
    
    public void PlaySFXCombatAudio(SFXCombatType sfxType, bool isStop = false, bool isPlayOneShot = false)
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
