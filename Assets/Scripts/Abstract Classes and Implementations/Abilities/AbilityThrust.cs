using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityThrust : AbstractAbility {
    public static string ID = "THRUST";

    public AbilityThrust() : base(
        ID,
        "Thrust",
        AbilityType.ATTACK,
        typeof(AbstractCharacter),
        1,
        0,
        1
    ){
        this.diceQueue.Add(new DiceEvade(1, 5));
        this.diceQueue.Add(new DiceAttack(2, 7));
    }

    public override void Subscribe(){
        CombatEventManager.OnAbilityUse += OnAbilityUse;
    }

    public override void Unsubscribe(){
        CombatEventManager.OnAbilityUse -= OnAbilityUse;
    }

    private void OnAbilityUse(AbstractAbility abilityUsed, AbilityTargeting target){
        if (abilityUsed == this){
            Debug.Log("MOVE THRUST");
            AbstractCharacter charTarget = (AbstractCharacter) target.GetTargeting();
            abilityUsed.abilityOwner.curLane = charTarget.curLane;
        }
    }
}
