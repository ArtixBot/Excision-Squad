using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility {

    public readonly string ID;
    public readonly string NAME;
    public readonly string DESC;
    public AbstractCharacter OWNER;
    public AbilityType TYPE;
    public int BASE_CD;
    // An ability consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    public List<AbilityDie> BASE_DICE = new List<AbilityDie>();

    public AbstractAbility(string ID, string NAME, string DESC, AbilityType TYPE, int BASE_CD){
        this.ID = ID;
        this.NAME = NAME;
        this.DESC = DESC;
        this.TYPE = TYPE;
        this.BASE_CD = BASE_CD;
    }

    public int curCooldown = 0;
    public List<AbilityDie> activeDice = new List<AbilityDie>();    // When an ability is used, copy over the contents of BASE_DICE into activeDice.

    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }
}