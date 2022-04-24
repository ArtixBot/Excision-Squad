using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionEvade : AbstractCombatAction {

    private AbstractCharacter winner;
    private int poiseRecovery;

    public CombatActionEvade(AbstractCharacter winner, int poise){
        this.winner = winner;
        this.poiseRecovery = poise;
    }

    public override void Resolve(){
        this.winner.curPoise += this.poiseRecovery;
        this.winner.curPoise = Mathf.Min(this.winner.curPoise, this.winner.maxPoise);
    }
}