using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionRecoverPoise : AbstractCombatAction {

    private AbstractCharacter target;
    private int poiseRecovery;

    public CombatActionRecoverPoise(AbstractCharacter target, int poise){
        this.target = target;
        this.poiseRecovery = poise;
    }

    public override void Resolve(){
        this.target.curPoise += this.poiseRecovery;
        this.target.curPoise = Mathf.Min(this.target.curPoise, this.target.maxPoise);
    }
}