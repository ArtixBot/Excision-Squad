using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityThrust : AbstractAbility {
    public static string ID = "THRUST";

    public AbilityThrust() : base(
        ID,
        "Thrust",
        AbilityType.ATTACK,
        1,
        0,
        1
    ){
        this.diceQueue.Add(new DiceEvade(1, 5));
        this.diceQueue.Add(new DiceAttack(2, 7));
    }

    public override void OnEquip(){
        CombatEventManager.OnAbilityUse += OnAbilityUse;
    }

    public override void OnUnequip(){
        CombatEventManager.OnAbilityUse -= OnAbilityUse;
    }

    private void OnAbilityUse(AbstractAbility abilityUsed){
        if (abilityUsed == this){
            Debug.Log("MOVE THRUST");
            abilityUsed.abilityOwner.curLane++;
        }
    }
}
