using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class TitleSceneManager : MonoBehaviour
    {
        public static TitleSceneManager Instance;
        [Header("[ Menus ]")]
        public BaseMenu MainMenu;
        public BaseMenu OptionMenu;

        public ESceneTypes NextSceneType;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            WorldAudioManager.Instance.PlayBGMAudio(0, BGMTypes.Title);
            if (OptionMenu.gameObject.activeSelf) OptionMenu.CloseMenuUI();
        }
    }
}
