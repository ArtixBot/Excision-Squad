using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public enum AbilityType { ATTACK, UTILITY };

public class AbilityTargeting {
    private object _target;

    public AbilityTargeting(AbstractCharacter singleTarget){ _target = singleTarget;}
    public AbilityTargeting(List<AbstractCharacter> multiTarget){ _target = multiTarget;}
    public AbilityTargeting(List<int> multiLane){_target = multiLane;}
    public AbilityTargeting(int lane){_target = lane;}

    public object GetTargeting() { return _target; }
}

public enum AbilityTags {
    /// <summary>If this ability would clash when used to initiate an attack, bypass the clash.</summary>
    SNEAKY
}

public abstract class AbstractAbility : ICombatEventSubscriber {
    
    public readonly string      ABILITY_ID;
    public readonly string      ABILITY_NAME;
    public readonly AbilityType ABILITY_TYPE;
    public readonly int         BASE_CD;
    public readonly int         BASE_MIN_RANGE;
    public readonly int         BASE_MAX_RANGE;
    public readonly int         BASE_VALOR_COST;
    public bool isAoE;

    // List of targeting condition modifiers (utility abilities, AoE Melee/Ranged attacks). Checking which units can be targeted by an ability is done by the CombatManager.
    public AbilityTargeting targetingType;
    protected List<AbstractDice> diceQueue = new List<AbstractDice>();     // An ability consists of a list of dice, played in order.
    public AbstractCharacter abilityOwner;
    public int curCooldown;     // Current cooldown of this ability. When this ability is successfully activated, increase cooldown by BASE_CD.

    public AbstractAbility(string ABILITY_ID, string ABILITY_NAME, AbilityType ABILITY_TYPE, Type targeting, int BASE_CD, int ABILITY_MIN_RANGE, int ABILITY_MAX_RANGE,
                            int VALOR_COST = 0,
                            List<AbilityTags> tags = null){
        this.ABILITY_ID = ABILITY_ID;
        this.ABILITY_NAME = ABILITY_NAME;
        this.ABILITY_TYPE = ABILITY_TYPE;
        this.BASE_CD = BASE_CD;
        this.BASE_MIN_RANGE = ABILITY_MIN_RANGE;
        this.BASE_MAX_RANGE = ABILITY_MAX_RANGE;
        this.BASE_VALOR_COST = VALOR_COST;
        this.isAoE = (this.targetingType.GetTargeting() is List<AbstractCharacter> || this.targetingType.GetTargeting() is List<int> || this.targetingType.GetTargeting() is int);
    }
    
    public virtual void Subscribe(){}
    public virtual void Unsubscribe(){}

    /// <summary>
    /// Check if this ability can be activated. This function can be overwritten to add additional conditions to check for.<br/>
    /// </summary>
    public virtual bool IsActivatable(){
        if (this.curCooldown > 0) { return false; }
        if (CombatManager.mapFactionToTeam[this.abilityOwner.CHAR_FACTION].Item2 < this.BASE_VALOR_COST) { return false; }
        return true;
    }


    /// <summary>Returns a deep copy of this ability's diceQueue.</summary>
    public List<AbstractDice> GetDice(){
        List<AbstractDice> deepCopy = new List<AbstractDice>();
        foreach(AbstractDice dice in this.diceQueue){
            AbstractDice newDiceCopy = dice.GetCopy();
            newDiceCopy.diceOwner = this.abilityOwner;
            deepCopy.Add(newDiceCopy);
        }
        return deepCopy;
    }
}