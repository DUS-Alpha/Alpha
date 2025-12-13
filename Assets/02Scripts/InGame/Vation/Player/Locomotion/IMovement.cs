using UnityEngine;

namespace alpha
{
    public interface IMovement
    {
        public bool CanMove();
        public bool CanRotate();
        public Vector3 Move(Vector2 moveInput, float currentSpeed, GameObject target);
        public void Rotate(Vector2 lookInput, float turnSmoothTime, GameObject target);
    }
}