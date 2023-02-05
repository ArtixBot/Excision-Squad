using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRiposte : AbstractAbility {
    
    public static string ability_id = "ATTACK_RIPOSTE";
    public static string ability_name = "Riposte";

    public AttackRiposte(){
        this.ABILITY_ID = ability_id;
        this.ABILITY_NAME = ability_name;

        AbilityDie initialBlock = new AbilityDie(DieType.BLOCK, 3, 8);
        AbilityDie followupAttack = new AbilityDie(DieType.MELEE, 4, 6);
        this.abilityDice.Add((initialBlock, new List<IEventListener>()));
        this.abilityDice.Add((followupAttack, new List<IEventListener>()));
    }

    private void blockSuccess(){
    }
}
