using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRiposte : AbstractAbility {
    
    public static string ability_id = "ATTACK_RIPOSTE";
    public static string ability_name = "Riposte";

    private AbilityDie initialBlockDie = new AbilityDie(DieType.BLOCK, 3, 8);
    private AbilityDie followupAttack = new AbilityDie(DieType.MELEE, 4, 6);
    private bool improveNextDie = false;

    public AttackRiposte(){
        this.ABILITY_ID = ability_id;
        this.ABILITY_NAME = ability_name;
        this.BASE_COOLDOWN = 1;

        this.abilityDice = new List<AbilityDie>{initialBlockDie, followupAttack};

        CombatEventManager.AddEventListener(CombatEventType.ON_DIE_ROLLED, this);
        CombatEventManager.AddEventListener(CombatEventType.ON_CLASH_WIN, this);
    }

    public override void HandleEvent(CombatEventData eventData){
        base.HandleEvent(eventData);

        if (eventData?.eventType == CombatEventType.ON_CLASH_WIN){
            CombatEventDataClashWin data = eventData as CombatEventDataClashWin;
            if (data.winningDie == this.initialBlockDie){ this.improveNextDie = true; }
        }
        else if (eventData?.eventType == CombatEventType.ON_DIE_ROLLED && !improveNextDie){
            CombatEventDataDieRolled data = eventData as CombatEventDataDieRolled;
            data.rolledValue += 2;
            this.improveNextDie = false;
        }
    }
}
