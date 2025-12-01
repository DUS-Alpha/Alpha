using System.Collections.Generic;
using UnityEngine;

public static class AnimID 
{
    //Trigger
    public static readonly int DoSubPatterm   = Animator.StringToHash("DoSubPattern");
    
    //int 
    public static readonly int Pattern   = Animator.StringToHash("Pattern");
    //Bool
    public static readonly int DoWalk   = Animator.StringToHash("DoWalk");
    
    
    public enum PatternID
    {
        /*
         * 1~10 : 근거리
         * 11~20 : 중거리
         * 21 ~30: 원거리
         */
        DodgeToUppercut = 1,
        RunBack = 2,
        SprintToJumpKick  = 11,
        JumpingBackKick = 12,
        DodgeLeftToFilpKick = 21,
        PowerPunch = 22,
        
    }
    
   
    
 
    
    public  static  List<PatternID> ClosePatternList = new List<PatternID>()
    {
        PatternID.DodgeToUppercut,
        PatternID.RunBack,
    };
    
    public static List<PatternID> MiddlePatternList = new List<PatternID>()
    {
        PatternID.DodgeLeftToFilpKick,
        PatternID.JumpingBackKick,
    };
    
    public static List<PatternID> FarPatternList = new List<PatternID>()
    {
        PatternID.SprintToJumpKick,
        PatternID.PowerPunch,
    };
    
    
    
}
