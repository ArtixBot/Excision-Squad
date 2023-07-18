using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState {
    COMBAT_START, COMBAT_END,
    ROUND_START, ROUND_END,
    TURN_START, TURN_END,
    AWAITING_CHARACTER_ABILITY,
    ONE_SIDED_ATTACK,
    CLASH,
    ABILITY_ACTIVATED
}

public static class CombatManager {

    private static CombatState _combatState;
    public static CombatState combatState {
        get => _combatState;
        set {_combatState = value; CombatManager.ResolveCombatState(_combatState);}
    }

    public static List<AbstractCharacter> participants = new List<AbstractCharacter>();
    public static Dictionary<CharacterFaction, List<AbstractCharacter>> combatantDict = new Dictionary<CharacterFaction, List<AbstractCharacter>>{
        {CharacterFaction.PLAYER, new List<AbstractCharacter>()},
        {CharacterFaction.ALLY, new List<AbstractCharacter>()},
        {CharacterFaction.NEUTRAL, new List<AbstractCharacter>()},
        {CharacterFaction.ENEMY, new List<AbstractCharacter>()}
    };

    public static ModdablePriorityQueue<AbstractCharacter> initiativeQueue = new ModdablePriorityQueue<AbstractCharacter>();
    public static int round;
    public static (AbstractCharacter character, int spd) activeChar;

    public static AbstractAbility attackingAbility;
    public static AbstractAbility defendingAbility;

    public static void ResolveCombatState(CombatState combatState){
        switch (combatState){
            case CombatState.COMBAT_START:
                StartCombat(CombatManager.participants);     // TODO: change this to take in a list of supplied fighters
                CombatManager.combatState = CombatState.ROUND_START;        // This will automatically invoke ResolveCombatState again.
                break;
            case CombatState.ROUND_START:
                StartRound(CombatManager.round);
                break;
            case CombatState.TURN_START:
                break;
            case CombatState.TURN_END:
                break;
            case CombatState.AWAITING_CHARACTER_ABILITY:
                break;
            case CombatState.ONE_SIDED_ATTACK:
                break;
            case CombatState.CLASH:
                break;
            default:
                break;
        }
    }
    
    public static void StartCombat(List<AbstractCharacter> fighters){
        foreach(AbstractCharacter character in fighters){
            combatantDict[character.CHAR_FACTION].Add(character);
        }
        Debug.Log("Combat starts!");
        CombatManager.round = 0;
        CombatEventManager.BroadcastEvent(new CombatEventCombatStart());
    }

    public static void StartRound(int round){
        foreach (CharacterFaction faction in combatantDict.Keys){
            foreach (AbstractCharacter character in combatantDict[faction]){
                for (int i = 0; i < character.actionsPerTurn; i++){
                    initiativeQueue.AddToQueue(character, 0);       // TODO: Change from 0
                }
            }
        }
        CombatManager.round += 1;
        CombatManager.activeChar = initiativeQueue.GetNextItem();
        Debug.Log($"Start Round {CombatManager.round}!");
    }

}