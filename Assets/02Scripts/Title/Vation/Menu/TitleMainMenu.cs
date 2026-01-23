using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class TitleMainMenu : BaseMenu
    {
        [Header("[ Buttons ]")]
        [SerializeField]
        private Button m_startButton;
        [SerializeField]
        private Button m_optionButton;

        protected override void Awake()
        {
            base.Awake();
            m_startButton.onClick.AddListener(PlayStartButton);
            m_optionButton.onClick.AddListener(OpenOptionMenu);
        }
        public void PlayStartButton()
        {
            SceneLoaderManager.Instance.SaveNextSceneAndLoadBootScene(TitleSceneManager.Instance.NextSceneType);
        }

        private void OpenOptionMenu()
        {
            CloseMenuUI();
            TitleSceneManager.Instance.OptionMenu.OpenMenuUI();
        }
    }
}