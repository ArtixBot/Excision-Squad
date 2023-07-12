using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EncounterData {
    public List<AbstractCharacter> incomingCombatants;
}

public static class CombatManager {
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

    public static void StartCombat(List<AbstractCharacter> fighters){
        foreach(AbstractCharacter character in fighters){
            combatantDict[character.CHAR_FACTION].Add(character);
        }
        CombatManager.round = 1;
        // CombatEventManager.combatStart?.Invoke();
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

    // public static void Clash(AbstractAbility attackingAbility, AbstractAbility defendingAbility){
    // }

    // private static void HandleAbilityActivated(AbstractAbility ability){
    //     ability.curCooldown = ability.BASE_CD;
    // }
}