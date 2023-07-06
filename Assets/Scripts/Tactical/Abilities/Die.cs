using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DieType {MELEE, RANGED, BLOCK, EVADE, UNIQUE};

/*
    The Die class forms the basis for the entire combat system.
    Abilities are composed of various amounts of dice, and when activated each die on that ability is rolled, with a corresponding action taken.
    AbstractDie contains:
        - (optional) dieId
        - DieType
        - ints for the minimum and maximum roll value
*/
public class Die {
    public readonly string dieId;
    public readonly DieType dieType;
    public readonly int minValue;
    public readonly int maxValue;

    public Die(DieType dieType, int minValue, int maxValue, string dieId = ""){
        this.dieId = dieId;
        this.dieType = dieType;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
    
    public int Roll(){
        return Random.Range(minValue, maxValue + 1);
    }

    public Die GetCopy(){
        return new Die(this.dieType, this.minValue, this.maxValue, this.dieId);
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