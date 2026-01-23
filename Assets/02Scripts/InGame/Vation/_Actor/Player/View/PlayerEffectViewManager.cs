using System.Collections;
using UnityEngine;

namespace alpha
{
    // TODO : 오브젝트풀링으로 관리
    public class PlayerEffectViewManager : MonoBehaviour, IEffectPort
    {
        // ==================== Dash
        [Header("[ Ref Config ]")]
        [SerializeField] private ParticleSystem m_frondWindEffect;
        [SerializeField] private SkinnedMeshRenderer[] m_skinnedRenderers;
        [SerializeField] private Material m_material;

        [Header("[ Value Config ]")]
        [SerializeField] private float m_activeTime = 0.5f;
        [SerializeField] private float m_destoryTime = 0.25f;
        [SerializeField] private float m_meshRefreshRate = 0.05f;

        private void OnValidate()
        {
            if (m_skinnedRenderers == null)
            {
                m_skinnedRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>();
            }
        }

        #region ==================== Dash ====================
        public void DashEffect()
        {
            m_frondWindEffect.Play();

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
                    gobj.transform.SetLocalPositionAndRotation(transform.position, transform.rotation);

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
        #endregion ==================== Dash ====================
    }
}