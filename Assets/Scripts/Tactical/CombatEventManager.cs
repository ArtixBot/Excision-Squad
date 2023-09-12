using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSubscriber {
    public int GetPriority();       // Higher values go first in execution order.
    public void HandleEvent(CombatEventData eventData);
}

public enum CombatEventType {
    ON_COMBAT_START, ON_COMBAT_END, // On combat start/end, an IEventObserver must subscribe/unsubscribe from all other events.
    ON_ROUND_START, ON_ROUND_END,
    ON_TURN_START, ON_TURN_END,
    ON_ABILITY_ACTIVATED,
    ON_UNIT_DEATH, ON_UNIT_DAMAGED,
    ON_DIE_ROLLED, ON_PRE_HIT,
    ON_CLASH_ELIGIBLE, ON_CLASH, ON_CLASH_WIN, ON_CLASH_TIE, ON_CLASH_LOSS,
    ON_STATUS_APPLIED, ON_STATUS_EXPIRED,
}

// Custom event handler for combat events.
public class CombatEventManager {
    public Dictionary<CombatEventType, ModdablePriorityQueue<IEventSubscriber>> events = new Dictionary<CombatEventType, ModdablePriorityQueue<IEventSubscriber>>();

    // Add a subscriber to the event dictionary if it's not already there. If a subscriber is added, return true; otherwise false.
    public bool Subscribe(CombatEventType eventType, IEventSubscriber subscriber){
        if (!events.ContainsKey(eventType)){
            events.Add(eventType, new ModdablePriorityQueue<IEventSubscriber>());
        }
        if (!events[eventType].ContainsItem(subscriber)){
            events[eventType].AddToQueue(subscriber, subscriber.GetPriority());
            return true;
        }
        return false;
    }

    // Notify all subscribers of a specified event.
    public void BroadcastEvent(CombatEventData eventData){
        if (!events.ContainsKey(eventData.eventType)) return;
        foreach ((IEventSubscriber subscriber, int _) in events[eventData.eventType].GetQueue()){
            subscriber.HandleEvent(eventData);
        }
    }

    // Return true if the subscriber is in the events dictionary for that event type, false otherwise.
    public bool CheckIfSubscribed(CombatEventType eventType, IEventSubscriber subscriber){
        if (!events.ContainsKey(eventType)) {return false;}
        return events[eventType].ContainsItem(subscriber);
    }

    // Remove a subscriber from a specified event type from the event dictionary.
    public void Unsubscribe(CombatEventType eventType, IEventSubscriber subscriber){
        if (!events.ContainsKey(eventType)){ return; }
        events[eventType].RemoveAllInstancesOfItem(subscriber);
    }

    // Remove the provided subscriber from all events. Used in instances such as when a character is defeated.
    public void UnsubscribeAll(IEventSubscriber subscriber){
        foreach (CombatEventType eventType in events.Keys){
            events[eventType].RemoveAllInstancesOfItem(subscriber);
        }
    }
}

public abstract class CombatEventData {
    public CombatEventType eventType;
}

public class CombatEventCombatStart : CombatEventData {
    public CombatEventCombatStart(){
        this.eventType = CombatEventType.ON_COMBAT_START;
    }
}

public class CombatEventCombatEnd : CombatEventData {
    public CombatEventCombatEnd(){
        this.eventType = CombatEventType.ON_COMBAT_END;
    }
}

public class CombatEventRoundStart : CombatEventData {
    public int roundStartNum;

    public CombatEventRoundStart(int roundStartNum){
        this.eventType = CombatEventType.ON_ROUND_START;
        this.roundStartNum = roundStartNum;
    }
}

public class CombatEventRoundEnd : CombatEventData {
    public int roundEndNum;

    public CombatEventRoundEnd(int roundEndNum){
        this.eventType = CombatEventType.ON_ROUND_START;
        this.roundEndNum = roundEndNum;
    }
}

public class CombatEventTurnStart : CombatEventData {
    public AbstractCharacter character;
    public int spd;

    public CombatEventTurnStart(AbstractCharacter character, int spd){
        this.character = character;
        this.spd = spd;
    }
}

public class CombatEventTurnEnd : CombatEventData {
    public AbstractCharacter character;
    public int spd;

    public CombatEventTurnEnd(AbstractCharacter character, int spd){
        this.character = character;
        this.spd = spd;
    }
}

public class CombatEventAbilityActivated : CombatEventData {
    public AbstractAbility abilityActivated;
    public List<Die> abilityDice;
    public AbstractCharacter caster;
    public AbstractCharacter target;
    public List<AbstractCharacter> targets = new List<AbstractCharacter>();

    public CombatEventAbilityActivated(AbstractCharacter caster, AbstractAbility abilityActivated, ref List<Die> abilityDice, AbstractCharacter target){
        this.eventType = CombatEventType.ON_ABILITY_ACTIVATED;
        this.abilityActivated = abilityActivated;
        this.abilityDice = abilityDice;
        this.caster = caster;
        this.target = target;
    }
    
    public CombatEventAbilityActivated(AbstractCharacter caster, AbstractAbility abilityActivated, ref List<Die> abilityDice, List<AbstractCharacter> targets){
        this.eventType = CombatEventType.ON_ABILITY_ACTIVATED;
        this.abilityActivated = abilityActivated;
        this.abilityDice = abilityDice;
        this.caster = caster;
        this.targets = targets;
    }
}

public class CombatEventDieRolled : CombatEventData {
    public Die die;
    public int rolledValue;

    public CombatEventDieRolled(Die die, ref int rolledValue){
        this.eventType = CombatEventType.ON_DIE_ROLLED;
        this.die = die;
        this.rolledValue = rolledValue;
    }
}

public class CombatEventClashEligible : CombatEventData {
    public AbstractCharacter reacter;
    public List<AbstractAbility> reacterEligibleAbilties;
    public AbstractCharacter attacker;

    ///<summary>Notify subscribers that a clash may occur.</summary>
    public CombatEventClashEligible(AbstractCharacter reacter, List<AbstractAbility> reacterEligibleAbilties, AbstractCharacter attacker){
        this.eventType = CombatEventType.ON_CLASH_ELIGIBLE;
        this.reacter = reacter;
        this.reacterEligibleAbilties = reacterEligibleAbilties;
        this.attacker = attacker;
    }
}

public class CombatEventClashOccurs : CombatEventData {
    public AbstractCharacter attacker;
    public AbstractAbility attackerAbility;
    public List<Die> attackerDice;

    public AbstractCharacter reacter;
    public AbstractAbility reacterAbility;
    public List<Die> reacterDice;

    ///<summary>Notify subscribers that a clash is occurring.</summary>
    public CombatEventClashOccurs(AbstractCharacter attacker, AbstractAbility attackerAbility, ref List<Die> attackerDice, AbstractCharacter reacter, AbstractAbility reacterAbility, ref List<Die> reacterDice){
        this.eventType = CombatEventType.ON_CLASH;
        this.attacker = attacker;
        this.attackerAbility = attackerAbility;
        this.attackerDice = attackerDice;

        this.reacter = reacter;
        this.reacterAbility = reacterAbility;
        this.reacterDice = reacterDice;
    }
}

public class CombatEventPreHit : CombatEventData {
    public AbstractCharacter attacker;
    public Die die;
    private int _damage;
    public int damage {
        get {return _damage;}
        set {_damage = damageCanBeFurtherModified ? value : _damage;}
    }
    public bool isPoiseDamage;
    public bool damageCanBeFurtherModified = true;      // Defaults to true, but can be false if the die in question is a restrained die.
    public AbstractCharacter target;

    ///<summary>Notify subscribers that a hit will occur (but the hit has not occurred yet).</summary>
    public CombatEventPreHit(AbstractCharacter attacker, Die die, ref int damage, bool isPoiseDamage, AbstractCharacter target){
        this.eventType = CombatEventType.ON_PRE_HIT;
        this.attacker = attacker;
        this.die = die;
        this.damage = damage;
        this.isPoiseDamage = isPoiseDamage;
        this.target = target;
    }
}