using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DieType {MELEE, RANGED, BLOCK, EVADE, UNIQUE};

/*
    The AbilityDie class forms the basis for the entire combat system.
    Abilities are composed of various amounts of dice, and when activated each die on that ability is rolled, with a corresponding action taken.
    AbstractDie contains:
        - (optional) dieId
        - DieType
        - ints for the minimum and maximum roll value
*/
public class AbilityDie {
    private string dieId;
    private DieType dieType;
    private int minValue;
    private int maxValue;

    public int Roll(int minValueMod = 0, int maxValueMod = 0){
        return Random.Range(minValue, maxValue + 1);
    }

    public AbilityDie(DieType dieType, int minValue, int maxValue, string dieId = ""){
        this.dieId = dieId;
        this.dieType = dieType;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public AbilityDie GetCopy(){
        return new AbilityDie(this.dieType, this.minValue, this.maxValue, this.dieId);
    }

    public DieType GetDieType(){
        return this.dieType;
    }

    public int GetMinValue(){
        return this.minValue;
    }

    public int GetMaxValue(){
        return this.maxValue;
    }
}