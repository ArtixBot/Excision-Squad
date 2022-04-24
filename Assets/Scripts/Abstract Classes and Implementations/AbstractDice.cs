using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType {ATTACK, BLOCK, EVADE};
public enum DiceMod {ON_HIT, ON_CLASH_LOSE, ON_CLASH_WIN, ON_CLASH};

public abstract class AbstractDice {
    public AbstractCharacter diceOwner;
    protected List<DiceMod> diceMods;
    protected DiceType diceType;
    protected int minValue;
    protected int maxValue;

    /// <summary> Roll dice with a range from [minValue + minValueMod, maxValue + maxValueMod].
    /// This function automatically adds relevant dice type modifiers (e.g. if rolling an attackDice, automatically also adds owner.attackRollMinMod and owner.attackRollMaxMod).</summary>.
    public abstract int Roll(int minValueMod = 0, int maxValueMod = 0);

    public abstract AbstractDice GetCopy();

    public new DiceType GetType(){
        return this.diceType;
    }
}

public class DiceAttack : AbstractDice {
    public DiceAttack(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.ATTACK;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }

    public override int Roll(int minValueMod = 0, int maxValueMod = 0){
                                                                    // null check
        int minThreshold = this.minValue + minValueMod + (this.diceOwner?.attackRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0);
        int maxThreshold = this.maxValue + maxValueMod + (this.diceOwner?.attackRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0);
        if (minThreshold > maxThreshold){           // If the max threshold is < the min threshold after modifier calculations, swap the min/max threshold.
            int tmp = maxThreshold;
            maxThreshold = minThreshold;
            minThreshold = tmp;
        }
        return Mathf.Max(0, Random.Range(minThreshold, maxThreshold + 1));    // roll cannot be negative
    }

    public override AbstractDice GetCopy(){
        return new DiceAttack(this.minValue, this.maxValue, this.diceMods);
    }
}

public class DiceBlock : AbstractDice {
    public DiceBlock(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.BLOCK;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }

    public override int Roll(int minValueMod = 0, int maxValueMod = 0){
        int minThreshold = this.minValue + minValueMod + (this.diceOwner?.blockRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0);
        int maxThreshold = this.maxValue + maxValueMod + (this.diceOwner?.blockRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0);
        if (minThreshold > maxThreshold){           // If the max threshold is < the min threshold after modifier calculations, swap the min/max threshold.
            int tmp = maxThreshold;
            maxThreshold = minThreshold;
            minThreshold = tmp;
        }
        return Mathf.Max(0, Random.Range(minThreshold, maxThreshold + 1));    // roll cannot be negative
    }
    
    public override AbstractDice GetCopy(){
        return new DiceBlock(this.minValue, this.maxValue, this.diceMods);
    }
}

public class DiceEvade : AbstractDice {
    public DiceEvade(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.EVADE;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }

    public override int Roll(int minValueMod = 0, int maxValueMod = 0){
        int minThreshold = this.minValue + minValueMod + (this.diceOwner?.evadeRollMinMod ?? 0) + (this.diceOwner?.allRollMinMod ?? 0);
        int maxThreshold = this.maxValue + maxValueMod + (this.diceOwner?.evadeRollMaxMod ?? 0) + (this.diceOwner?.allRollMaxMod ?? 0);
        if (minThreshold > maxThreshold){           // If the max threshold is < the min threshold after modifier calculations, swap the min/max threshold.
            int tmp = maxThreshold;
            maxThreshold = minThreshold;
            minThreshold = tmp;
        }
        return Mathf.Max(0, Random.Range(minThreshold, maxThreshold + 1));    // roll cannot be negative
    }

    public override AbstractDice GetCopy(){
        return new DiceEvade(this.minValue, this.maxValue, this.diceMods);
    }
}