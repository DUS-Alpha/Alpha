using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CinemachineSplineDolly))]
public class CinemachineController : MonoBehaviour
{
    [Header("완주 시 한 번 호출")]
    public UnityEvent onCompleted;   // 인스펙터에서 BossActions.OpenSpecialGate 등 연결

    CinemachineSplineDolly dolly;
    bool isPlaying;

    void Awake()
    {
        dolly = GetComponent<CinemachineSplineDolly>();

        // 자동 이동 사용(Inspector: Method = Fixed Speed, Speed 값 설정)
        dolly.AutomaticDolly.Enabled = true;
        
    }

    void OnEnable()
    {
        // 매번 시작점을 0으로
        dolly.CameraPosition = 0f;
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

        // Normalized 기준: 1(끝)에 도달하면 완료
        if (dolly.CameraPosition >= 1f)
            OnArrived();
    }

    void OnArrived()
    {
        isPlaying = false;
        dolly.AutomaticDolly.Enabled = false;   // 더 이상 자동 이동 안 하도록 OFF
        onCompleted?.Invoke();                  // SpecialPattern 진입 신호 등
        enabled = false;                        // 스크립트 자체 비활성화(원하면 제거)
    }

    // 필요 시 외부에서 다시 재생할 때 사용
    public void PlayAgain()
    {
        enabled = true;
        dolly.AutomaticDolly.Enabled = true;
        dolly.CameraPosition = 0f;
        isPlaying = true;
    }
}