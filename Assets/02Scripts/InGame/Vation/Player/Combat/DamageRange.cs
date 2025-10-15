using System;
using UnityEngine;

public class DamageRange : MonoBehaviour
{
    [SerializeField]
    private Collider m_rangeCollider;
    public float damage;

    [Tooltip("시작지점 index = 0 "),SerializeField]
    private Transform[] m_movePoint;  // 시작지점x 이동지점만 적용

    [SerializeField]
    private float[] m_moveTime;     // 각 포인트간의 이동 시간
    private float m_moveT;
    private int m_index;
    private bool m_isMoving;
    private void Start()
    {
        m_rangeCollider.enabled = false;
        m_index = 1;
    }

    /// <summary>
    /// 데미지 영역이 투사체일경우 true로
    /// </summary>
    /// <param name="isMove"></param>
    public void OnCollider(bool isMove = false)
    {
        m_isMoving = isMove;
        m_rangeCollider.enabled = true;
        
        m_index = 0;
        m_moveT = 0f;
    }
    public void OffCollider()
    {
        m_rangeCollider.enabled = false;
    }
    private void Update()
    {
        if (!m_isMoving) return;

        if (m_index >= m_movePoint.Length - 1)
        {
            // 모든 구간 끝 → 이동 종료
            transform.position = m_movePoint[0].position; // 시작 위치 고정
            m_isMoving = false;
            m_rangeCollider.enabled = false;
            return;
        }

        m_moveT += Time.deltaTime;

        // 현재 구간 진행도 (0 ~ 1)
        float t = Mathf.Clamp01(m_moveT / m_moveTime[m_index]);

        // 현재 구간의 시작점과 끝점
        Vector3 start = m_movePoint[m_index].position;
        Vector3 end = m_movePoint[m_index + 1].position;

        // 보간 이동
        transform.position = Vector3.Lerp(start, end, t);

        if (t >= 1f)
        {
            m_index++;
            m_moveT = 0;
        }
    }
    public bool TryGetTarget(out RaycastHit hit, float maxDistance)
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;

        bool isHit = Physics.Raycast(origin, dir, out hit, maxDistance, 1 << LayerMask.NameToLayer("Enemy"));

        return isHit;
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))

        if (other.gameObject.CompareTag("BossMeleeRange"))
        {
            /*IDamageable damageable;
            if (other.TryGetComponent<IDamageable>(out damageable))
            {
                DamageMassage _damageMassage = new DamageMassage();
                _damageMassage.damage = 10;
                damageable.ApplyDamage(_damageMassage);
            }*/
            Debug.Log("ggg");
            if (other.TryGetComponent<HitBox>(out HitBox _hitBox))
            {
                DamageMassage _damageMassage = new DamageMassage();
                //_damageMassage.Damager = damager;
                //_damageMassage.HitNormal = hit.normal;
                //_damageMassage.HitPoint = hit.point;
                //RangeWeapon _range = CurrentWeapon as RangeWeapon;
                _damageMassage.damage = damage;

                _hitBox.damageable.ApplyDamage(_damageMassage);
                print("히트박스 데미지 완료");

            }
        }
    }
}
