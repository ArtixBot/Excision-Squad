using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility {

    public int ABILITY_ID;
    public int ABILITY_NAME;
    public AbstractCharacter ABILITY_OWNER;
    public AbilityType ABILITY_TYPE;
    public int BASE_COOLDOWN;

    public int curCooldown;

    // An ability consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    public List<(AbilityDie, List<int>)> abilityDice = new List<(AbilityDie, List<int>)>();

    // Some abilities have an "On Use:" modifier.
    public virtual void OnActivate(){
        this.curCooldown = this.BASE_COOLDOWN;
    }

    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }
}