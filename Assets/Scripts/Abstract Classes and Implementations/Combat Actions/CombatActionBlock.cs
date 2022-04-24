using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionBlock : AbstractCombatAction {

    private AbstractCharacter victim;
    private int poiseDamage;

    public CombatActionBlock(AbstractCharacter defender, int baseDamage){
        this.victim = defender;
        this.poiseDamage = baseDamage;
    }

    public override void Resolve(){
        this.victim.curPoise -= this.poiseDamage;
        if (this.victim.curPoise <= 0){
            // TODO: become Staggered
        }
    }
}