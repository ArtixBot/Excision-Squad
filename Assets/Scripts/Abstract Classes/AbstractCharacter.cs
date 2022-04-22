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
    public int minSpeed, maxSpeed;          // At the start of each round, for each action the user has, roll for speed between [minSpeed, maxSpeed] + speedMod
    
    public int speedMod = 0;    // Modifier to the speed value of the user
    public int allRollMinMod = 0, allRollMaxMod = 0;        // Modifier to the min/max roll range for ALL dice
    public int attackRollMinMod = 0, attackRollMaxMod = 0;  // Modifier to the min/max roll range for an attack dice
    public int blockRollMinMod = 0, blockRollMaxMod = 0;    // Modifier to the min/max roll range for a block dice
    public int evadeRollMinMod = 0, evadeRollMaxMod = 0;    // Modifier to the min/max roll range for an evade dice

    public List<AbstractAbility> abilities = new List<AbstractAbility>();

    public bool HasActionsRemaining(){
        return CombatManager.turnQueue.ContainsCharacter(this);
    }
}

public class CharacterDeckard : AbstractCharacter{

    public CharacterDeckard(){
        this.minSpeed = 1;
        this.maxSpeed = 6;
    }
}