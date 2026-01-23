using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class BootSceneManager : MonoBehaviour
    {
        [SerializeField]
        private Image m_progressBar;

        private void Start()
        {
            m_progressBar.fillAmount = 0;
            ESceneTypes _nextScene = SceneLoaderManager.Instance.NextSceneTyep;
            SceneLoaderManager.Instance.LoadNextScene_Async(_nextScene, m_progressBar);
        }
    }
}
