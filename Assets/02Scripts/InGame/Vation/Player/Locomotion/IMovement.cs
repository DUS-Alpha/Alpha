using UnityEngine;

namespace alpha
{
    public interface IMovement
    {
        public bool CanMove();
        public bool CanRotate();
        public void Move(Vector2 moveInput, bool isRun, MoveConfig moveConfig, GameObject target);
        public void Rotate(Vector2 lookInput, RotateConfig rotateConfig, GameObject target);
    }
}