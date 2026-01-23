using System;
using UnityEngine;

namespace alpha
{
    public interface ILocomoAniPort
    {
        float GetCurrentAniInfo(int index);
        void InitializeMoveAni();
        void MoveAni(float inputX, float inputY, bool isCombat);
        void SetMoveType(bool isCombat);
        void JumpAni();
        void FallAni(EFallType fallType);
        void LandAni(EFallType fallType);
        void DashAni();
        void FlyUpAni();
        void FlightMoveAni(float inputX, float inputY, bool isCombat);
        void SetFlightMoveType(bool isCombat);
    }
}