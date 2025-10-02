using System;
using UnityEngine;


//실재 위치가 어디인지 씬에서 보이게 만드느 기능
[ExecuteAlways] // 에디터 실행을 안해도 바로 실행 
public class BossDebugGizmos : MonoBehaviour
{
    public BossAction actions;
    public Color MidColor = new Color(0.2f, 1f, 0.4f, 0.4f);
    public Color CloseColor = new Color(1f, 0.3f, 0.3f, 0.4f);
    public Color StopColor = Color.magenta;
    public Color lineColor = Color.yellow;


  

    private void OnDrawGizmos()
    {
        if (!actions) actions = GetComponent<BossAction>();
        if (!actions) return;
        
        // 실제로 움직이는 기준(Animator가 손자라면 그 트랜스폼)
        
        
        // 원 그리기
        DrawCircle(actions.animator.transform.position, actions.MidRange, MidColor);
        DrawCircle(actions.animator.transform.position, actions.CloseRange, CloseColor);
        DrawCircle(actions.BB.Target.position, actions.StopRange, StopColor);
        

        // 타깃 방향선
        if (actions.BB != null && actions.BB.Target)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(actions.animator.transform.position, actions.BB.Target.position);
        }
    }

    private void DrawCircle(Vector3 center, float radius, Color c, int seg = 48)
    {
        if (radius <= 0f) return;
        Gizmos.color = c;
        Vector3 prev = center + Vector3.right * radius;
        for (int i = 1; i <= seg; i++)
        {
            float t = (i / (float)seg) * Mathf.PI * 2f;
            Vector3 next = center + new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}