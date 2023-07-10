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
    ON_DIE_ROLLED, ON_HIT,
    ON_CLASH, ON_CLASH_WIN, ON_CLASH_TIE, ON_CLASH_LOSS,
    ON_STATUS_APPLIED, ON_STATUS_EXPIRED,
}

public abstract class CombatEventData {
    public CombatEventType eventType;
}

// Custom event handler for combat events.
public static class CombatEventManager {
    public static Dictionary<CombatEventType, ModdablePriorityQueue<IEventSubscriber>> events = new Dictionary<CombatEventType, ModdablePriorityQueue<IEventSubscriber>>();

    // Add a subscriber to the event dictionary if it's not already there. If a subscriber is added, return true; otherwise false.
    public static bool Subscribe(CombatEventType eventType, IEventSubscriber subscriber){
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
    public static void BroadcastEvent(CombatEventData eventData){
        if (!events.ContainsKey(eventData.eventType)) return;
        foreach (IEventSubscriber subscriber in events[eventData.eventType]){
            subscriber.HandleEvent(eventData);
        }
    }

    // Return true if the subscriber is in the events dictionary for that event type, false otherwise.
    public static bool CheckIfSubscribed(CombatEventType eventType, IEventSubscriber subscriber){
        if (!events.ContainsKey(eventType)) {return false;}
        return events[eventType].ContainsItem(subscriber);
    }

    // Remove a subscriber from a specified event type from the event dictionary.
    public static void Unsubscribe(CombatEventType eventType, IEventSubscriber subscriber){
        if (!events.ContainsKey(eventType)){ return; }
        events[eventType].RemoveAllInstancesOfItem(subscriber);
    }

    // Remove the provided subscriber from all events. Used in instances such as when a character is defeated.
    public static void UnsubscribeAll(IEventSubscriber subscriber){
        foreach (CombatEventType eventType in events.Keys){
            events[eventType].RemoveAllInstancesOfItem(subscriber);
        }
    }
}