using alpha;
using UnityEngine;

public class MeleeWeaponStrategy : IAttackStrategy
{
    // 로코모션에 전달 필요 (이벤트로)
    public bool CanMoveDuringAttack => false;
    private int currentComboNum;

    public void StartAttack(PlayerCombat combat)
    {
        // 시작시는 0부터 시작
        currentComboNum = combat.NextComboNum;
        combat.AniM.MeleeComboAni(combat.NextComboNum);
    }

    public void UpdateAttack(PlayerCombat combat)
    {
        if(combat.IsNextCombo)
        {
            if(currentComboNum != combat.NextComboNum)
            {
                combat.AniM.MeleeComboAni(combat.NextComboNum);
                currentComboNum = combat.NextComboNum;
            }
        }
    }
    public void ExitAttack(PlayerCombat combat)
    {
        currentComboNum = 0;
    }
}
