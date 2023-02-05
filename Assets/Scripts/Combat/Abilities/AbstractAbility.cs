using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility : IEventListener {

    public string ABILITY_ID;
    public string ABILITY_NAME;
    public AbstractCharacter ABILITY_OWNER;
    public AbilityType ABILITY_TYPE;
    public int BASE_COOLDOWN;

    public int curCooldown;

    // An ability consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    public List<(AbilityDie, List<IEventListener>)> abilityDice = new List<(AbilityDie, List<IEventListener>)>();

    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }

    public virtual void HandleEvent(CombatEventData eventData){
        switch (eventData?.eventType) {
            case CombatEventType.ON_ABILITY_ACTIVATED:
                CombatEventDataAbilityActivated data = (CombatEventDataAbilityActivated) eventData;
                if (data.abilityActivated == this){
                    this.curCooldown = this.BASE_COOLDOWN;
                }
                break;
            default:
                break;
        }
    }
}