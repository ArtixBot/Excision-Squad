using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionDamage : AbstractCombatAction {

    private AbstractCharacter attacker;
    private AbstractCharacter victim;
    private int damageDealt;

    public CombatActionDamage(AbstractCharacter attacker, AbstractCharacter defender, int baseDamage){
        this.attacker = attacker;
        this.victim = defender;
        this.damageDealt = Mathf.RoundToInt((((baseDamage + attacker.damageAddMod) * attacker.damageMultMod) + defender.damageTakenAddMod) * defender.damageTakenMultMod);
    }

    public override void Resolve(){
        this.victim.curHP -= this.damageDealt;
        if (this.victim.curHP <= 0){
            CombatEventManager.InvokeCharDeath(this.attacker, this.victim);
        }
    }
}
