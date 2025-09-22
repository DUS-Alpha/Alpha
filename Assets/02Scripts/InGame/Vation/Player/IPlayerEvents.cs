using System;
using UnityEngine;

public interface IPlayerEvents
{
    public event Action CheckInputAction;
    public event Action<int> SwapWeaponAction;
}
