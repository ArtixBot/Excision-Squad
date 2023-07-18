using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {ATTACK, REACTION, UTILITY};

public abstract class AbstractAbility : IEventSubscriber {

    public string ID;
    public string NAME;
    public string DESC;
    public AbstractCharacter OWNER;
    public AbilityType TYPE;
    public int BASE_CD;
    public bool IS_GENERIC;         // Characters can't equip more than 4 IS_GENERIC abilities.
    public int MIN_RANGE;           
    public int MAX_RANGE;
    
    // An attack/reaction consists of a list of dice and any events (e.g. on hit, on clash, on clash win, on clash lose, etc.) associated with that die.
    // On attack/reaction activation, copy the list of BASE_DICE to CombatManager's attacker/defender dice queue.
    public List<Die> BASE_DICE = new List<Die>();

    public int curCooldown = 0;
    
    public AbstractAbility(string ID, string NAME, string DESC, AbilityType TYPE, int BASE_CD, int MIN_RANGE, int MAX_RANGE){
        this.ID = ID;
        this.NAME = NAME;
        this.DESC = DESC;
        this.TYPE = TYPE;
        this.BASE_CD = BASE_CD;
        this.MIN_RANGE = MIN_RANGE;
        this.MAX_RANGE = MAX_RANGE;
    }

    public bool IsAvailable(){
        return this.curCooldown <= 0;
    }

    public bool IsUnavailable(){
        return this.curCooldown > 0;
    }

    public virtual int GetPriority(){ return 400; }
    public virtual void HandleEvent(CombatEventData eventData){
        CombatEventType eventType = eventData.eventType;
        switch (eventType){
            case CombatEventType.ON_ABILITY_ACTIVATED:
                CombatEventAbilityActivated data = (CombatEventAbilityActivated) eventData;
                if (data.abilityActivated == this){
                    this.Activate();
                }
                break;
            default:
                break;
        }
    }

    private void Activate(){
        this.curCooldown = this.BASE_CD;
    }
}