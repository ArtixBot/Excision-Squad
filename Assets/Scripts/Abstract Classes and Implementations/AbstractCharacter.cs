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
    public int minSpd, maxSpd;          // At the start of each round, for each action the user has, roll for speed between [minSpeed, maxSpeed] + speedMod
    
    public int speedMod = 0;    // Modifier to the speed value of the user
    public int allRollMinMod = 0, allRollMaxMod = 0;        // Modifier to the min/max roll range for ALL dice
    public int attackRollMinMod = 0, attackRollMaxMod = 0;  // Modifier to the min/max roll range for an attack dice
    public int blockRollMinMod = 0, blockRollMaxMod = 0;    // Modifier to the min/max roll range for a block dice
    public int evadeRollMinMod = 0, evadeRollMaxMod = 0;    // Modifier to the min/max roll range for an evade dice
    
    public int damageAddMod = 0;
    public float damageMultMod = 1.0f;

    public int damageTakenAddMod = 0;
    public float damageTakenMultMod = 1.0f;

    public List<AbstractAbility> abilities = new List<AbstractAbility>();

    public AbstractCharacter(string ID, string NAME, CharacterFaction FACTION, int maxHP, int maxPoise, int actionsPerRound, int minSpd, int maxSpd){
        this.CHAR_ID = ID;
        this.CHAR_NAME = NAME;
        this.CHAR_FACTION = FACTION;
        this.maxHP = maxHP;
        this.curHP = this.maxHP;
        this.maxPoise = maxPoise;
        this.curPoise = this.maxPoise;
        this.actionsPerRound = actionsPerRound;
        this.minSpd = minSpd;
        this.maxSpd = maxSpd;
    }

    public bool HasActionsRemaining(){
        return CombatManager.turnQueue.ContainsCharacter(this);
    }
}

public class CharacterDeckard : AbstractCharacter{

    private static string ID = "CHAR_DECKARD";
    private static int initialMaxHP = 20, initialMaxPoise = 20, initialMaxActions = 1;
    private static int initialMinSpd = 1, initialMaxSpd = 6;

    public CharacterDeckard() : base(CharacterDeckard.ID,
                                    "Deckard",
                                    CharacterFaction.ALLY,
                                    initialMaxHP,
                                    initialMaxPoise,
                                    initialMaxActions,
                                    initialMinSpd,
                                    initialMaxSpd){}
}