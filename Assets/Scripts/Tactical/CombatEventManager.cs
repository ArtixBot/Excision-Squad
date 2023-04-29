using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatEventType {
    ON_COMBAT_START, ON_COMBAT_END, // On combat start/end, an IEventObserver must subscribe/unsubscribe from all other events.
    ON_ROUND_START, ON_ROUND_END,
    ON_TURN_START, ON_TURN_END,
    ON_ABILITY_ACTIVATED,
    ON_UNIT_DEATH, ON_UNIT_DAMAGED,
    ON_DIE_ROLLED, ON_DIE_HIT,
    ON_CLASH, ON_CLASH_WIN, ON_CLASH_TIE, ON_CLASH_LOSS,
    ON_STATUS_APPLIED, ON_STATUS_EXPIRED,
}

public interface IEventObserver {
    int eventPriority {get;}            // Higher values trigger AFTER lower values.
    void HandleEvent(CombatEventData eventData);
}

// Custom event handler for combat events.
public static class CombatEventManager {
    private static Dictionary<CombatEventType, ModdablePriorityQueue<IEventObserver>> observers = new Dictionary<CombatEventType, ModdablePriorityQueue<IEventObserver>>{
        // Core events
        {CombatEventType.ON_COMBAT_START, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_COMBAT_END, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_ROUND_START, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_ROUND_END, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_TURN_START, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_TURN_END, new ModdablePriorityQueue<IEventObserver>()},
        // Generic combat events
        {CombatEventType.ON_ABILITY_ACTIVATED, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_UNIT_DEATH, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_UNIT_DAMAGED, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_STATUS_APPLIED, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_STATUS_EXPIRED, new ModdablePriorityQueue<IEventObserver>()},
        // Attack events
        {CombatEventType.ON_DIE_ROLLED, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_DIE_HIT, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_CLASH, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_CLASH_WIN, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_CLASH_TIE, new ModdablePriorityQueue<IEventObserver>()},
        {CombatEventType.ON_CLASH_LOSS, new ModdablePriorityQueue<IEventObserver>()},
    };

    public static void AddEventListener(CombatEventType eventType, IEventObserver eventListener){
        ModdablePriorityQueue<IEventObserver> observers = CombatEventManager.observers[eventType];
        if (!observers.ContainsItem(eventListener)){
            observers.AddToQueue(eventListener, eventListener.eventPriority);
        }
    }

    public static void RemoveEventListener(CombatEventType eventType, IEventObserver eventListener){
        ModdablePriorityQueue<IEventObserver> observers = CombatEventManager.observers[eventType];
        observers.RemoveAllInstancesOfItem(eventListener);
    }

    public static void TriggerEvent(CombatEventType eventType, CombatEventData eventData = null){
        foreach (IEventObserver observer in CombatEventManager.observers[eventType]){
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

public class CombatEventDataRoundStart : CombatEventData{
    public int newRoundValue;
    public CombatEventDataRoundStart(int roundValue){
        this.newRoundValue = roundValue;
        this.eventType = CombatEventType.ON_ROUND_START;
    }
}

public class CombatEventDataRoundEnd : CombatEventData{
    public int endRoundValue;
    public CombatEventDataRoundEnd(int roundValue){
        this.endRoundValue = roundValue;
        this.eventType = CombatEventType.ON_ROUND_END;
    }
}

public class CombatEventDataDieRolled : CombatEventData{
    public AbilityDie rolledDie;
    public int rolledValue;

    /// <summary>Combat event data for a rolled die. Includes the die that was rolled, as well as the rolled value (which is passed by reference).</summary>
    public CombatEventDataDieRolled(AbilityDie rolledDie, ref int rolledVal){
        this.eventType = CombatEventType.ON_CLASH_WIN;
        this.rolledDie = rolledDie;
        this.rolledValue = rolledVal;
    }
}

public class CombatEventDataClash : CombatEventData{
    public AbilityDie attackingDie;
    public int attackingRoll;
    public AbilityDie defendingDie;
    public int defendingRoll;

    public CombatEventDataClash(AbilityDie atkDie, ref int atkRoll, AbilityDie defDie, ref int defRoll){
        this.eventType = CombatEventType.ON_CLASH;
        this.attackingDie = atkDie;
        this.attackingRoll = atkRoll;
        this.defendingDie = defDie;
        this.defendingRoll = defRoll;
    }
}

public class CombatEventDataClashWin : CombatEventData{
    public AbilityDie winningDie;
    public int winningValue;
    public AbilityDie losingDie;
    public int losingValue;

    public CombatEventDataClashWin(AbilityDie winner, ref int winVal, AbilityDie loser, ref int lossVal){
        this.eventType = CombatEventType.ON_CLASH_WIN;
        this.winningDie = winner;
        this.winningValue = winVal;
        this.losingDie = loser;
        this.losingValue = lossVal;
    }
}

public class CombatEventDataClashLoss : CombatEventData{
    public AbilityDie winningDie;
    public int winningValue;
    public AbilityDie losingDie;
    public int losingValue;

    public CombatEventDataClashLoss(AbilityDie winner, ref int winVal, AbilityDie loser, ref int lossVal){
        this.eventType = CombatEventType.ON_CLASH_LOSS;
        this.winningDie = winner;
        this.winningValue = winVal;
        this.losingDie = loser;
        this.losingValue = lossVal;
    }
}

public class CombatEventDataTurnStart : CombatEventData{
    public AbstractCharacter character;

    public CombatEventDataTurnStart(AbstractCharacter character){
        this.eventType = CombatEventType.ON_TURN_START;
        this.character = character;
    }
}

public class CombatEventDataTurnEnd : CombatEventData{
    public AbstractCharacter character;

    public CombatEventDataTurnEnd(AbstractCharacter character){
        this.eventType = CombatEventType.ON_TURN_END;
        this.character = character;
    }
}

public class CombatEventDataClashTie : CombatEventData{
    public CombatEventDataClashTie(){
        this.eventType = CombatEventType.ON_CLASH_TIE;
    }
}


public class CombatEventDataAbilityActivated : CombatEventData{
    public AbstractAbility abilityActivated;

    public CombatEventDataAbilityActivated(AbstractAbility abilityActivated){
        this.eventType = CombatEventType.ON_ABILITY_ACTIVATED;
        this.abilityActivated = abilityActivated;
    }
}