using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionDamage : AbstractCombatAction {

    private AbstractCharacter victim;
    private int damageDealt;
    private int poiseDamageDealt;

    public CombatActionDamage(AbstractCharacter attacker, AbstractCharacter defender, int baseDamage){
        this.victim = defender;
        this.damageDealt = Mathf.RoundToInt((((baseDamage + attacker.damageAddMod) * attacker.damageMultMod) + defender.damageTakenAddMod) * defender.damageTakenMultMod);
    }

    public override void Resolve(){
        this.victim.curHP -= this.damageDealt;
        this.victim.curPoise -= this.damageDealt;
        if (this.victim.curHP <= 0){
            // TODO: die
        }
        if (this.victim.curPoise <= 0){
            // TODO: stagger
        }
    }
}
