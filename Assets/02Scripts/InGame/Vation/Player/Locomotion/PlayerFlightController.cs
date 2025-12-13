using alpha;
using UnityEngine;

namespace alpha
{
    public class PlayerFlightController : IMovement
    {
        private CharacterController m_characterCtrl;
        public PlayerFlightController(CharacterController characterCtrl)
        {
            m_characterCtrl = characterCtrl;
        }

        private float m_currentSmoothVelocityX = 0;
        private float m_currentSmoothVelocityY = 0;
        private float m_currentSmoothVelocityZ = 0;

        public bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        public bool CanRotate()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 Move(Vector2 moveInput, float currentSpeed, GameObject target)
        {
            if (moveInput == Vector2.zero) return Vector3.zero;
            
            Vector3 _dir = target.transform.right * moveInput.x + target.transform.forward * moveInput.y;

            if (_dir.sqrMagnitude > 1f) _dir.Normalize();

            m_characterCtrl.Move(_dir * Time.deltaTime * currentSpeed);
            return _dir;
        }

        public void Rotate(Vector2 lookInput, float turnSmoothTime, GameObject target)
        {
            if (lookInput == Vector2.zero)
            {
                return;
            }

            Vector3 _targetRot = Camera.main.transform.eulerAngles; // 카메라 정면을 기준으로만 회전

            Vector3 _currentEuler = target.transform.eulerAngles;

            float smoothX = Mathf.SmoothDampAngle(_currentEuler.x,_targetRot.x,ref m_currentSmoothVelocityX, turnSmoothTime);

            float smoothY = Mathf.SmoothDampAngle(_currentEuler.y,_targetRot.y,ref m_currentSmoothVelocityY, turnSmoothTime);

            float smoothZ = Mathf.SmoothDampAngle(_currentEuler.z,_targetRot.z,ref m_currentSmoothVelocityZ, turnSmoothTime);

            target.transform.rotation = Quaternion.Euler(smoothX, smoothY, smoothZ);
        }
    }
}