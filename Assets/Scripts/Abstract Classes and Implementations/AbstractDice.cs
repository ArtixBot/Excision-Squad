using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType {ATTACK, BLOCK, EVADE, UNIQUE};

/*
    The AbstractDice class forms the basis for the entire combat system.
    Abilities are composed of various amounts of dice, and when activated each die on that ability is rolled and a corresponding action is taken.
    AbstractDice contain:
        - DiceType
        - ints for the minimum and maximum roll value
        - an AbstractCharacter reference (might be unnecessary).
        - A die implements ICombatEventSubscriber. Its parent ability handles subscribing / unsubscribing from events.
*/
public abstract class AbstractDice : ICombatEventSubscriber {
    protected DiceType diceType;
    protected int minValue;
    protected int maxValue;
    public AbstractCharacter diceOwner;

    /// <summary> Roll dice with a range from [minValue + minValueMod, maxValue + maxValueMod].
    /// This function automatically adds relevant dice type modifiers (e.g. if rolling an attackDice, automatically also adds owner.attackRollMinMod and owner.attackRollMaxMod).</summary>.
    public int Roll(int minValueMod = 0, int maxValueMod = 0){
        int minThreshold, maxThreshold;
        switch (this.diceType){
            case DiceType.ATTACK:
                /*
                    Thresholds can never go below zero.
                    We zero out negative values here instead of during the actual roll to prevent RNG from being overly influenced towards negative/zero values
                    when we have a negative minThreshold and positive maxThreshold.
                */
                minThreshold = Mathf.Max(this.minValue + minValueMod + (this.diceOwner?.attackRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0), 0);
                maxThreshold = Mathf.Max(this.maxValue + maxValueMod + (this.diceOwner?.attackRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0), 0);
                break;
            case DiceType.BLOCK:
                minThreshold = Mathf.Max(this.minValue + minValueMod + (this.diceOwner?.blockRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0), 0);
                maxThreshold = Mathf.Max(this.maxValue + maxValueMod + (this.diceOwner?.blockRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0), 0);
                break;
            case DiceType.EVADE:
                minThreshold = Mathf.Max(this.minValue + minValueMod + (this.diceOwner?.evadeRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0), 0);
                maxThreshold = Mathf.Max(this.maxValue + maxValueMod + (this.diceOwner?.evadeRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0), 0);
                break;
            default:
                minThreshold = Mathf.Max(this.minValue + minValueMod, 0);
                maxThreshold = Mathf.Max(this.maxValue + maxValueMod, 0);
                break;
        }
        if (minThreshold > maxThreshold){           // Handle cases where roll modifiers cause minThreshold > maxThreshold.
            maxThreshold = minThreshold;
        }
        return Random.Range(minThreshold, maxThreshold + 1);    // roll cannot be negative
    }

    public abstract AbstractDice GetCopy();

    public abstract void TriggerEffect(AbstractCharacter owner, AbstractCharacter target, int value);

    public virtual void Subscribe(){}
    public virtual void Unsubscribe(){}

    public new DiceType GetType(){
        return this.diceType;
    }
}

public class DiceAttack : AbstractDice {
    public DiceAttack(int minValue, int maxValue){
        this.diceType = DiceType.ATTACK;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public override AbstractDice GetCopy(){
        return new DiceAttack(this.minValue, this.maxValue);
    }

    public override void TriggerEffect(AbstractCharacter owner, AbstractCharacter target, int value){
        CombatManager.AddActionToQueue(new CombatActionDamage(owner, target, value));
        CombatManager.AddActionToQueue(new CombatActionPoiseDamage(owner, target, value));
    }
}

public class DiceBlock : AbstractDice {
    public DiceBlock(int minValue, int maxValue){
        this.diceType = DiceType.BLOCK;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
    
    public override AbstractDice GetCopy(){
        return new DiceBlock(this.minValue, this.maxValue);
    }

    public override void TriggerEffect(AbstractCharacter owner, AbstractCharacter target, int value){
        CombatManager.AddActionToQueue(new CombatActionPoiseDamage(owner, target, value));
    }
}

public class DiceEvade : AbstractDice {
    public DiceEvade(int minValue, int maxValue){
        this.diceType = DiceType.EVADE;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public override AbstractDice GetCopy(){
        return new DiceEvade(this.minValue, this.maxValue);
    }

    public override void TriggerEffect(AbstractCharacter owner, AbstractCharacter target, int value){
        CombatManager.AddActionToQueue(new CombatActionRecoverPoise(owner, value));
    }
}