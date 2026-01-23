using System;
using UnityEngine;

namespace alpha
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField] private PlayerCore m_playerCore;
        [SerializeField] private RealTimeUIManager m_realTimeUIM;
        [SerializeField] private WindowUIManager m_windowUIM;

        void Start()
        {
            WorldAudioManager.Instance.PlayBGMAudio(0, BGMTypes.InGame);
            WorldCameraManager.Instance.SetCursor(false);
        }

        public void HandleInvenotryUI()
        {

        }

        public void LeaveInGame()
        {
            SceneLoaderManager.Instance.SaveNextSceneAndLoadBootScene(ESceneTypes.Title);
        }

    }
}