using System.Collections.Generic;
using UnityEngine;

public class CombatFlagsRules : MonoBehaviour
{
    public HashSet<LocomotionStateType> AimAllowedStates = new HashSet<LocomotionStateType>
    {
        LocomotionStateType.Idle,
        LocomotionStateType.Move,
        LocomotionStateType.Jump,
        LocomotionStateType.Fall,
        LocomotionStateType.FlightMove  // 나중에 상태 추가 시 바로 넣으면 됨
    };

    public HashSet<LocomotionStateType> ReloadAllowedStates = new HashSet<LocomotionStateType>
    { 
        LocomotionStateType.Idle,
        LocomotionStateType.Move,
        LocomotionStateType.Jump,
        LocomotionStateType.Fall,
        LocomotionStateType.FlightMove
    };

    public HashSet<LocomotionStateType> SwapAllowedStates = new HashSet<LocomotionStateType>
    { 
        LocomotionStateType.Idle,
        LocomotionStateType.Move,
        LocomotionStateType.Jump,
        LocomotionStateType.Fall,
        LocomotionStateType.FlightMove
    };
}
