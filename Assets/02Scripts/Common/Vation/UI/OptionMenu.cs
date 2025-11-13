using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace alpha
{
    public enum EAudioMixerType { Master, BGM, SFX }
    public class OptionMenu : BaseMenu
    {
        [Header("[ Buttons ]")]
        [SerializeField]
        private Button m_exitButton;

        [Header("[ Audio Settings ]")]
        [SerializeField] 
        private Slider m_volumeSlider_Master;
        [SerializeField]
        private Slider m_volumeSlider_BGM;
        [SerializeField]
        private Slider m_volumeSlider_SFX;

        protected override void Awake()
        {
            base.Awake();
            
            m_exitButton.onClick.AddListener(ReturnMainMenu);
            InitializeSlider();
        }

        private void InitializeSlider()
        {
            m_volumeSlider_Master.onValueChanged.AddListener(SetMasterChangeVolume);
            m_volumeSlider_BGM.onValueChanged.AddListener(SetBGMChangeVolume);
            m_volumeSlider_SFX.onValueChanged.AddListener(SetSFXChangeVolume);
        }

        private void Start()
        {
            m_volumeSlider_Master.value = WorldAudioManager.Instance.GetVolume_Master;
            m_volumeSlider_BGM.value = WorldAudioManager.Instance.GetVolume_BGM;
            m_volumeSlider_SFX.value = WorldAudioManager.Instance.GetVolume_SFX;

            SetMasterChangeVolume(m_volumeSlider_Master.value);
            SetBGMChangeVolume(m_volumeSlider_BGM.value);
            SetSFXChangeVolume(m_volumeSlider_SFX.value);
        }

        public void ReturnMainMenu()
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)ESceneTypes.InGame) return;
                CloseMenuUI();
        }

        public void SetMasterChangeVolume(float volume)
        {
            WorldAudioManager.Instance.SetVolume_Master = volume;
        }
        public void SetBGMChangeVolume(float volume)
        {
            WorldAudioManager.Instance.SetVolume_BGM = volume;
        }
        public void SetSFXChangeVolume(float volume)
        {
            WorldAudioManager.Instance.SetVolume_SFX = volume;
        }
    }
}