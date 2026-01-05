using UnityEngine;

namespace alpha
{
    public class LocomotionController : MonoBehaviour
    {
        private ILocomotionInput m_input;

        public void Bind(ILocomotionInput input)
        {
            m_input = input;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}