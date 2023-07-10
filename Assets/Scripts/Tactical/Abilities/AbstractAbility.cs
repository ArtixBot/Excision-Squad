using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility {

    public readonly string ID;
    public string NAME;
    public string DESC;
    public AbstractCharacter OWNER;
    public AbilityType TYPE;
    public int BASE_CD;
    public bool IS_GENERIC;         // Characters can't equip more than 4 IS_GENERIC abilities.
    
    // An attack/reaction consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    // On attack/reaction activation, copy the list of BASE_DICE to CombatManager's attacer/defender dice queue.
    public List<Die> BASE_DICE = new List<Die>();

    public AbstractAbility(string ID, string NAME, string DESC, AbilityType TYPE, int BASE_CD){
        this.ID = ID;
        this.NAME = NAME;
        this.DESC = DESC;
        this.TYPE = TYPE;
        this.BASE_CD = BASE_CD;
    }

    public int curCooldown = 0;
    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }

    public void Activate(){
        this.curCooldown = this.BASE_CD;
    }
}