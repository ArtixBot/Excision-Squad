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

    public static InitiativeQueue initiativeQueue = new InitiativeQueue();
    public static int round;

    public static void SetUp(EncounterData encounterData){
        foreach(AbstractCharacter character in encounterData.incomingCombatants){
            combatantDict[character.CHAR_FACTION].Add(character);
        }
        CombatManager.round = 0;
        CombatManager.StartRound();
    }

    public static void StartRound(){
        foreach (CharacterFaction faction in combatantDict.Keys){
            foreach (AbstractCharacter character in combatantDict[faction]){
                for (int i = 0; i < character.actionsPerTurn; i++){
                    initiativeQueue.AddCombatantToQueue(character);
                }
            }
        }
        CombatManager.round += 1;
    }

    public static void CleanUp(){
        combatantDict.Clear();
    }
}