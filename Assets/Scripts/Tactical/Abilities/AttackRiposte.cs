using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRiposte : AbstractAbility {
    
    public static string ability_id = "ATTACK_RIPOSTE";
    public static string ability_name = "Riposte";

    private bool improveNextDie = false;

    private AbilityDie initialBlockDie = new AbilityDie(DieType.BLOCK, 3, 8);
    private AbilityDie followupAttack = new AbilityDie(DieType.MELEE, 4, 6);

    public AttackRiposte(){
        this.ABILITY_ID = ability_id;
        this.ABILITY_NAME = ability_name;
        this.BASE_COOLDOWN = 1;

        this.abilityDice = new List<AbilityDie>{initialBlockDie, followupAttack};
    }
}
