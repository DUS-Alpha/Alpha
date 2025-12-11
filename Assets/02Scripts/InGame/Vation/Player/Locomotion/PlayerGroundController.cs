using alpha;
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

        public void Move(Vector2 moveInput,bool isRun ,MoveConfig moveConfig, GameObject target)
        {
            if (moveInput == Vector2.zero)
            {
                return;
            }
            Vector3 _dir = target.transform.right * moveInput.x + target.transform.forward * moveInput.y;

            if (_dir.sqrMagnitude > 1f) _dir.Normalize();

            m_characterCtrl.Move(_dir * Time.deltaTime * moveConfig.CurrentSpeed);
        }

        public void Rotate(Vector2 lookInput, RotateConfig rotateConfig, GameObject target)
        {
            if(lookInput == Vector2.zero)
            {
                return;
            }

            float _targetRot = Camera.main.transform.eulerAngles.y; // 카메라 정면을 기준으로만 회전
            
            Vector3 _currentEuler = target.transform.eulerAngles;

            target.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_currentEuler.y, _targetRot,
                                    ref m_currentSmoothVelocityY, rotateConfig.TurnSmoothTime);

        }
    }
}