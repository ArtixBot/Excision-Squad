using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMove : AbstractAbility {
    public static string ID = "MOVE";

    public AbilityMove() : base(
        ID,
        "Move",
        AbilityType.UTILITY,
        typeof(int),
        0,
        1,
        1
    ){
        CombatEventManager.OnAbilityUse += HealWhenUsed;
    }

    private void HealWhenUsed(AbstractAbility abilityUsed, AbilityTargeting target){
        if (abilityUsed == this){
            abilityUsed.abilityOwner.curLane = (int)target.GetTargeting();
        }
    }
}