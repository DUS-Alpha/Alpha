using System;
using UnityEngine;

namespace alpha
{
    public class InGameManager : MonoBehaviour
    {
        void Start()
        {
            WorldAudioManager.Instance.PlayBGMAudio(0, BGMTypes.InGame);
            WorldCameraManager.Instance.SetCursor(false);
        }

        public void LeaveInGame()
        {
            SceneLoaderManager.Instance.SaveNextSceneAndLoadBootScene(ESceneTypes.Title);
        }
    }
}