using UnityEngine;

namespace alpha
{
    public class PlayerInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerStateMachineManager m_stateM;
        [SerializeField] private PlayerInputManager m_inputM;
        [SerializeField] private LocomotionController m_locomotionController;
        private PlayerCore m_playerCore;
        private void Awake()
        {
            m_playerCore = new PlayerCore();
            m_locomotionController.Bind(m_inputM);

        }
    }
}