using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//빨/흰 박스는 같은 부모, 같은 중심(pivot 0.5, 0.5)으로 둘것
public class ShrinkTimingQTE : MonoBehaviour
{

    public UnityEvent onSuccess; // 성공 카운팅 위한 이벤트
    public UnityEvent onFailure; // 성공 카운팅 위한 이벤트
    public RectTransform white;      // 줄어드는 박스
    public RectTransform red;        // 목표 박스(고정)
    public KeyCode key ;

    public float duration = 2f;      // 1.0 -> endScale까지 줄어드는 시간
    public float endScale = 0.1f;    // 최종 스케일
    [Range(0f, 0.5f)]
    public float tolerance = 0.05f;  // 허용 오차(스케일 기준)

    float t;
    float targetScale;               // 빨간/흰 초기 폭 비율
    bool playing;

    void Start()
    {
        targetScale = red.rect.width / white.rect.width; // 중심/피벗 동일 가정
        Restart();
    }

    public void Restart()
    {
        t = 0f;
        playing = true;
        white.localScale = Vector3.one;
    }

    void Update()
    {
        if (!playing) return;

        // 줄이기
        t += Time.deltaTime / duration;
        float s = Mathf.Lerp(1f, endScale, t);
        white.localScale = new Vector3(s, s, 1f);

        // 입력 판정
        if (Input.GetKeyDown(key))
        {
            if (Mathf.Abs(s - targetScale) <= tolerance)
            {
                Debug.Log("SUCCESS");
                onSuccess?.Invoke();      // ★ 이벤트 호출
            }
            else if (s > targetScale + tolerance)
            {
                onFailure?.Invoke();
                Debug.Log("FAIL (too early)");
            }
            else
            {
                onFailure?.Invoke();
               Debug.Log("FAIL (too late)");
            }

            

            playing = false;
            StartCoroutine(Hide());
        }

        // 시간 초과(끝까지 줄어듦)
        if (t >= 1f)
        {
            Debug.Log("FAIL (timeout)");
            playing = false;
            onFailure?.Invoke();

            StartCoroutine(Hide());
        }
        
        
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
