using UnityEngine;


//실재 위치가 어디인지 씬에서 보이게 만드느 기능
[ExecuteAlways]
public class BossDebugGizmos : MonoBehaviour
{
    public BossActions actions;
    public Color shootColor = new Color(0.2f, 0.7f, 1f, 0.4f);
    public Color optimalColor = new Color(0.2f, 1f, 0.4f, 0.4f);
    public Color tooCloseColor = new Color(1f, 0.3f, 0.3f, 0.4f);
    public Color lineColor = Color.yellow;

    private void OnDrawGizmos()
    {
        if (!actions) actions = GetComponent<BossActions>();
        if (!actions) return;

        // 원 그리기
        DrawCircle(transform.position, actions.ShootRange, shootColor);
        DrawCircle(transform.position, actions.OptimalRange, optimalColor);
        DrawCircle(transform.position, actions.TooCloseRange, tooCloseColor);

        // 타깃 방향선
        if (actions.BB != null && actions.BB.Target)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, actions.BB.Target.position);
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