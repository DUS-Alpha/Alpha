using System.Collections.Generic;
using UnityEngine;

// TODO : 현재는 전투중심의 시스템을 먼저 개발
public class PlayerInventoryManager : MonoBehaviour
{
    public WeaponItemSO CurrentRightHandWeapon;
    public WeaponItemSO CurrentLeftHandWeapon;

    public void InitializeModule(PlayerCore playerCore)
    {
        
    }

    public void InitializeEvents(IPlayerEvents events)
    {

    }
}
