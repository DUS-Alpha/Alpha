using UnityEngine;

namespace alpha
{
    public interface ILocomotionInput
    {
        public Vector2 MoveDirInput { get; }
        public bool IsMove { get; }

        public Vector2 LookInput { get; }
        public bool IsRotLock { get; }
        public bool IsDash { get; }
        public bool IsJump { get; }
        public bool IsFlyUp { get; }
    }
}