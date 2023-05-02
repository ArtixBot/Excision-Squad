using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility {

    public string ABILITY_ID;
    public string ABILITY_NAME;
    public string ABILITY_DESC;
    public AbstractCharacter ABILITY_OWNER;
    public AbilityType ABILITY_TYPE;
    public int BASE_COOLDOWN;

    public int curCooldown = 0;

    // An ability consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    public List<AbilityDie> abilityDice = new List<AbilityDie>();

    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }
}