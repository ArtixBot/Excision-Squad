using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterFaction {ALLY, ENEMY};

public abstract class AbstractCharacter {
    public string CHAR_ID;
    public string CHAR_NAME;
    public CharacterFaction CHAR_FACTION;

    public int maxHP, curHP;
    public int maxPoise, curPoise;

    public int actionsPerRound;
    public int minSpeed, maxSpeed, speedMod;          // At the start of each round, for each action the user has, roll for initiative between [minSpeed, maxSpeed] + speedMod
    
    public List<AbstractAbility> abilities = new List<AbstractAbility>();
}