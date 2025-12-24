using UnityEngine;

namespace alpha
{
    // 모든 저장 파일에 대해 이 데이터를 참조하려고 하므로 이 스크립트는 단일 동작이 아니며 대신 직렬화 가능합니다.
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("[ Character ItemName ]")]
        public string CharacterName;

        [Header("Time Played")]
        public float SecondsPlayed;

        // 저장 시 Vector방식이 아니라 기본적인 값의 방식으로 데이터 저장 그래야 차후 데이터 테이블 등에 대해서 관리 용이
        [Header("[ World Coordinates ]")]
        public float XPos;
        public float YPos;
        public float ZPos;

    }
}