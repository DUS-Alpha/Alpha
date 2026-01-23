using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace alpha
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance;

        [SerializeField]
        private PlayerCore m_playerCore;

        [SerializeField] 
        private bool m_isSaveGame;
        [SerializeField] 
        private bool m_isLoadGame;

        public int WorldSceneIndex;



        [Header("[ Save Data Writer ]")]
        private SaveFileDataWriter m_saveDataWriter;

        [Header("[ Current Character Data ]")]
        public CharacterSlots CurrentCharacterSlotBeingUsed;
        public CharacterSaveData CurrentCharacterData;
        private string m_saveFileName;


        [Header("[ Character Slots ]")]
        public CharacterSaveData CharacterData01;
        /*public CharacterSaveData CharacterData02;
        public CharacterSaveData CharacterData03;
        public CharacterSaveData CharacterData04;
        public CharacterSaveData CharacterData05;
        public CharacterSaveData CharacterData06;
        public CharacterSaveData CharacterData07;
        public CharacterSaveData CharacterData08;
        public CharacterSaveData CharacterData09;
        public CharacterSaveData CharacterData10;*/
        
        public int GetWorldSceneIndex => SceneManager.GetActiveScene().buildIndex;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        private void Update()
        {
            // TODO : UI로
            if(m_isSaveGame)
            {
                m_isSaveGame = false;
                SaveGame();
            }
            if(m_isLoadGame)
            {
                m_isSaveGame = false;
                LoadGame();
            }
        }

        public void NewGame()
        {
            // 슬롯에 따라 파일 이름을 지정하여 새 파일을 만듭니다.
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            CurrentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            // 슬롯에 따라 파일 이름이 달라지는 이전 파일을 로드합니다.
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            m_saveDataWriter = new SaveFileDataWriter();
            // 일반적으로 여러 머신 유형(Application.persistentDataPath)에서 작동합니다.
            m_saveDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            m_saveDataWriter.SaveFileName = m_saveFileName;

            // m_saveDataWriter.SaveFileName으로 저장된 파일 로드
            CurrentCharacterData = m_saveDataWriter.LoadSaveFile();


        }

        public void SaveGame()
        {
            // 현재 파일을 사용 중인 슬롯에 따라 파일 이름으로 저장합니다.
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            m_saveDataWriter = new SaveFileDataWriter();
            // 일반적으로 여러 머신 유형(Application.persistentDataPath)에서 작동합니다.
            m_saveDataWriter.SaveDataDirectoryPath = Application.persistentDataPath;
            m_saveDataWriter.SaveFileName = m_saveFileName;

            // 게임에서 플레이어의 저장 파일로 정보를 전달합니다.
            //m_playerCore.SaveGameDataToCurrentCharacterData(ref CurrentCharacterData);

            // 현재 캐릭터데이터의 파일 저장
            m_saveDataWriter.CreateNewCharacterSaveFile(CurrentCharacterData);
        }

        public void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (CurrentCharacterSlotBeingUsed)
            {
                case CharacterSlots.CharacterSlot_01:
                    m_saveFileName = "CharacterSlot01";
                    break;
                case CharacterSlots.CharacterSlot_02:
                    m_saveFileName = "CharacterSlot02";
                    break;
                case CharacterSlots.CharacterSlot_03:
                    m_saveFileName = "CharacterSlot03";
                    break;
                case CharacterSlots.CharacterSlot_04:
                    m_saveFileName = "CharacterSlot04";
                    break;
                case CharacterSlots.CharacterSlot_05:
                    m_saveFileName = "CharacterSlot05";
                    break;
                case CharacterSlots.CharacterSlot_06:
                    m_saveFileName = "CharacterSlot06";
                    break;
                case CharacterSlots.CharacterSlot_07:
                    m_saveFileName = "CharacterSlot07";
                    break;
                case CharacterSlots.CharacterSlot_08:
                    m_saveFileName = "CharacterSlot08";
                    break;
                case CharacterSlots.CharacterSlot_09:
                    m_saveFileName = "CharacterSlot09";
                    break;
                case CharacterSlots.CharacterSlot_10:
                    m_saveFileName = "CharacterSlot10";
                    break;
                case CharacterSlots.NO_SLOT:
                    break;
            }
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation _operation = SceneManager.LoadSceneAsync(WorldSceneIndex);

            yield return null;
        }

    }
}