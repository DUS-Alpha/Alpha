using System;
using System.IO;
using UnityEngine;

namespace alpha
{
    [Serializable]
    public class SaveFileDataWriter : MonoBehaviour
    {
        public string SaveDataDirectoryPath = "";
        public string SaveFileName = "";

        // 새로운 저장 파일을 생성하기 전에 해당 캐릭터 슬롯 중 같은 이름 파일 존재 유무 파악 (최대 10개 캐릭터 슬롯)
        public bool CheckToSeeIfFileExists()
        {
            // SaveFileName 존재한다.
            if (File.Exists(Path.Combine(SaveDataDirectoryPath, SaveFileName))) return true;
            else return false;
        }

        // 해당 파일 삭제
        public void DeleteSaaveFile()
        {
            File.Delete(Path.Combine(SaveDataDirectoryPath, SaveFileName));
        }

        // 게임 저장 파일 생성
        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData)
        {
            string _savePath = Path.Combine(SaveDataDirectoryPath, SaveFileName);

            try
            {
                // 해당 경로에 폴더 생성
                Directory.CreateDirectory(Path.GetDirectoryName(_savePath));

                // C# 데이터 객체를 JSON으로 직렬화합니다. (유니티(characterSaveData) -> Json 형태로 변환)
                string _dataToStore = JsonUtility.ToJson(characterSaveData);

                // 파일 생성 및 _dataToStore 내용 파일에 작성
                using (FileStream stream = new FileStream(_savePath, FileMode.Create))
                {
                    using (StreamWriter filewriter = new StreamWriter(stream))
                    {
                        filewriter.Write(_dataToStore);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.LogError("Game Not Create Saved File : " + ex.Message);
            }
        }

        // 저장 파일 로드
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData _characterData = null;

            string _loadPath = Path.Combine(SaveDataDirectoryPath, SaveFileName);


            if(File.Exists(_loadPath))
            {
                try
                {
                    string _dataToLoad = "";

                    using (FileStream stream = new FileStream(_loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            _dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // Json -> Unity(characterData) 데이터로 전환
                    _characterData = JsonUtility.FromJson<CharacterSaveData>(_dataToLoad);
                }
                catch(Exception ex)
                {
                    Debug.LogError($"{ex.Message}");
                }
            }
            return _characterData;
        }


    }
}
