using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionAttack : AbstractCombatAction {

    private AbstractCharacter victim;
    private int damageToDeal;

    public CombatActionAttack(AbstractCharacter attacker, AbstractCharacter defender, int baseDamage){
        this.victim = defender;
        this.damageToDeal = Mathf.RoundToInt((((baseDamage + attacker.damageAddMod) * attacker.damageMultMod) + defender.damageTakenAddMod) * defender.damageTakenMultMod);
    }

    public override void Resolve(){
        this.victim.curHP -= this.damageToDeal;
        if (this.victim.curHP <= 0){
            // TODO: die
        }
    }
}
