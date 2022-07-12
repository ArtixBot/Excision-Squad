using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMove : AbstractAbility {
    public static string ID = "MOVE";

    public AbilityMove() : base(
        ID,
        "Move",
        AbilityType.UTILITY,
        0,
        1,
        0
    ){
        CombatEventManager.OnAbilityUse += HealWhenUsed;
    }

    private void HealWhenUsed(AbstractAbility abilityUsed){
        Debug.Log("TEST");
    }
}