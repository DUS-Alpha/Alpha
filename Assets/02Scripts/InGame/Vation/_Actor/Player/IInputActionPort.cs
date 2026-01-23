using System;

namespace alpha
{
    public interface IInputActionPort
    {
        event Action<PlayerInputManager> OnInputAction;
    }
}