using UnityEngine;

namespace alpha
{
    public class PlayerGroundController : IMovement
    {
        private CharacterController m_characterCtrl;
        public PlayerGroundController(CharacterController characterCtrl)
        {
            m_characterCtrl = characterCtrl;
        }

        private float m_currentSmoothVelocityY = 0;
        public bool CanMove()
        {
            throw new System.NotImplementedException();
        }

        public bool CanRotate()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 Move(Vector2 moveInput ,float currentSpeed, GameObject target)
        {
            if (moveInput == Vector2.zero) return Vector3.zero;
            
            Vector3 _dir = target.transform.right * moveInput.x + target.transform.forward * moveInput.y;

            if (_dir.sqrMagnitude > 1f) _dir.Normalize();

            m_characterCtrl.Move(_dir * Time.deltaTime * currentSpeed);
            return _dir;
        }

        public void Rotate(Vector2 lookInput, float turnSmoothTime, GameObject target)
        {
            if(lookInput == Vector2.zero) return;
            
            float _targetRot = Camera.main.transform.eulerAngles.y; // 카메라 정면을 기준으로만 회전
            
            Vector3 _currentEuler = target.transform.eulerAngles;

            target.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_currentEuler.y, _targetRot,
                                    ref m_currentSmoothVelocityY, turnSmoothTime);

        }
    }
}