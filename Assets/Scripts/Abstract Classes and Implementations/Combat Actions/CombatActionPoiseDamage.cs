using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionPoiseDamage : AbstractCombatAction {

    private AbstractCharacter victim;
    private int poiseDamageDealt;

    public CombatActionPoiseDamage(AbstractCharacter attacker, AbstractCharacter defender, int baseDamage){
        this.victim = defender;
        this.poiseDamageDealt = Mathf.RoundToInt((((baseDamage + attacker.damageAddMod) * attacker.damageMultMod) + defender.damageTakenAddMod) * defender.damageTakenMultMod);
    }

    public override void Resolve(){
        this.victim.curPoise -= this.poiseDamageDealt;
        if (this.victim.curPoise <= 0){
            // TODO: stagger
        }
    }
}
