using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Button m_startButton;
        [SerializeField]
        private SceneTypes m_nextSceneType;
        private void Awake()
        {
            m_startButton.onClick.AddListener(PlayStartButton);
        }

        public void PlayStartButton()
        {
            SceneLoaderManager.Instance.SaveNextSceneAndLoadBootScene(m_nextSceneType);
        }
    }
}
