using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneTypes
{
    Title,
    Boot,
    InGame
}

namespace alpha
{
    public class SceneLoaderManager : MonoBehaviour
    {
        public static SceneLoaderManager Instance;

        [SerializeField]
        private float m_progressBarSpeed;

        public SceneTypes NextSceneTyep { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void LoadNextScene(SceneTypes sceneType)
        {
            SceneManager.LoadScene((int)sceneType);
        }

        public void SaveNextSceneAndLoadBootScene(SceneTypes sceneType)
        {
            LoadNextScene(SceneTypes.Boot);
            NextSceneTyep = sceneType;
        }
        public void LoadNextScene_Async(SceneTypes sceneType, Image progressBar)
        {
            StartCoroutine(AsyncLoadSceneCoroutine(sceneType, progressBar));
        }

        public IEnumerator AsyncLoadSceneCoroutine(SceneTypes sceneType, Image image)
        {
            AsyncOperation _operation = SceneManager.LoadSceneAsync((int)sceneType);
            _operation.allowSceneActivation = false;

            float _timer = 0f;
            while (!_operation.isDone)
            {
                yield return null;

                if (_operation.progress < 0.9f) continue;

                _timer += Time.deltaTime * m_progressBarSpeed;
                image.fillAmount = Mathf.Lerp(0, 1f, _timer);
                if (image.fillAmount >= 1f)
                {
                    _operation.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}