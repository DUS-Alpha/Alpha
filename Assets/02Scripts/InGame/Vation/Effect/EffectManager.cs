using System.Collections;
using UnityEngine;

// TODO : 오브젝트풀링으로 관리
public class EffectManager : MonoBehaviour
{
    // ==================== Dash
    [SerializeField]
    private GameObject m_target;
    [SerializeField]
    private SkinnedMeshRenderer[] m_skinnedRenderers;
    [SerializeField]
    private Material m_material;

    [SerializeField]
    private float m_activeTime = 0.5f;
    [SerializeField]
    private float m_destoryTime = 1f;
    [SerializeField]
    private float m_meshRefreshRate = 0.1f;

    // ==================== Dash
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DashEffect()
    {
        StartCoroutine(ActiveTrailEffectCoroutine(m_activeTime));
    }
    private IEnumerator ActiveTrailEffectCoroutine(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= m_meshRefreshRate;
            for (int i = 0; i < m_skinnedRenderers.Length; i++)
            {
                GameObject gobj = new GameObject();
                gobj.transform.SetLocalPositionAndRotation(m_target.transform.position, m_target.transform.rotation);

                MeshRenderer _mr = gobj.AddComponent<MeshRenderer>();
                MeshFilter _mf = gobj.AddComponent<MeshFilter>();

                Mesh _mesh = new Mesh();
                m_skinnedRenderers[i].BakeMesh(_mesh);
                _mf.mesh = _mesh;
                _mr.material = m_material;

                Destroy(gobj, m_destoryTime);
            }



            yield return new WaitForSeconds(m_meshRefreshRate);
        }
    }

    private void OnValidate()
    {
        if (m_skinnedRenderers == null)
        {
            m_skinnedRenderers = m_target.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }
}
