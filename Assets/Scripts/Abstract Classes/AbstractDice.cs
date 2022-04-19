using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType {ATTACK, BLOCK, EVADE};
public enum DiceMod {ON_HIT, ON_CLASH_LOSE, ON_CLASH_WIN, ON_CLASH};

public abstract class AbstractDice {
    protected List<DiceMod> diceMods;
    protected DiceType diceType;
    protected int minValue;
    protected int maxValue;

    // Roll the dice with a range from [minValue + minValueMod, maxValue + maxValueMod].
    public int Roll(int minValueMod = 0, int maxValueMod = 0){
        return Random.Range(this.minValue + minValueMod, this.maxValue + maxValueMod + 1);
    }
}

public class DiceAttack : AbstractDice {
    public DiceAttack(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.ATTACK;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }
}

public class DiceBlock : AbstractDice {
    public DiceBlock(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.BLOCK;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }
}

public class DiceEvade : AbstractDice {
    public DiceEvade(int minValue, int maxValue, List<DiceMod> diceMods = null){
        this.diceType = DiceType.EVADE;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.diceMods = diceMods;
    }
}