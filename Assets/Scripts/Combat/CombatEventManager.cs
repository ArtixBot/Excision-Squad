using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatEventType {
    ON_COMBAT_START,                // On combat start, an IEventListener must perform all other event subscriptions.
    ON_COMBAT_END,                  // On combat end, an IEventListener must unsubscribe from all other events.
    ON_ROUND_START,
    ON_ROUND_END,
    ON_TURN_START,
    ON_TURN_END,
    ON_ABILITY_ACTIVATED,
    ON_DIE_ROLLED,
    ON_CLASH,
    ON_CLASH_WIN,
    ON_CLASH_TIE,
    ON_CLASH_LOSS,
    ON_UNIT_KILLED,
}

public interface IEventListener {
    void HandleEvent(CombatEventData eventData);
}

// Custom event handler for combat events.
public static class CombatEventManager {
    private static Dictionary<CombatEventType, List<IEventListener>> observers = new Dictionary<CombatEventType, List<IEventListener>>{
        {CombatEventType.ON_COMBAT_START, new List<IEventListener>()},
        {CombatEventType.ON_COMBAT_END, new List<IEventListener>()},
        {CombatEventType.ON_ROUND_START, new List<IEventListener>()},
        {CombatEventType.ON_ROUND_END, new List<IEventListener>()},
        {CombatEventType.ON_ABILITY_ACTIVATED, new List<IEventListener>()},
        {CombatEventType.ON_DIE_ROLLED, new List<IEventListener>()},
        {CombatEventType.ON_CLASH, new List<IEventListener>()},
        {CombatEventType.ON_CLASH_WIN, new List<IEventListener>()},
        {CombatEventType.ON_CLASH_LOSS, new List<IEventListener>()},
        {CombatEventType.ON_UNIT_KILLED, new List<IEventListener>()},
    };

    public static void AddEventListener(CombatEventType eventType, IEventListener eventListener){
        List<IEventListener> observers = CombatEventManager.observers[eventType];
        if (!observers.Contains(eventListener)){
            observers.Add(eventListener);
        }
    }

    public static void RemoveEventListener(CombatEventType eventType, IEventListener eventListener){
        List<IEventListener> observers = CombatEventManager.observers[eventType];
        observers.Remove(eventListener);
    }

    public static void TriggerEvent(CombatEventType eventType, CombatEventData eventData = null){
        foreach (IEventListener observer in CombatEventManager.observers[eventType]){
            observer.HandleEvent(eventData);
        }
    }
}

public abstract class CombatEventData{
    public CombatEventType eventType;
}

public class CombatEventDataCombatStart : CombatEventData{
    public CombatEventDataCombatStart(){
        this.eventType = CombatEventType.ON_COMBAT_START;
    }
}

public class CombatEventDataCombatEnd : CombatEventData{
    public CombatEventDataCombatEnd(){
        this.eventType = CombatEventType.ON_COMBAT_END;
    }
}

public class CombatEventDataAbilityActivated : CombatEventData{
    public AbstractAbility abilityActivated;

    public CombatEventDataAbilityActivated(AbstractAbility abilityActivated){
        this.eventType = CombatEventType.ON_ABILITY_ACTIVATED;
        this.abilityActivated = abilityActivated;
    }
}