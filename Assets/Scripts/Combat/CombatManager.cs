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
    public static (AbstractCharacter character, int spd) activeChar;

    public static AbstractAbility attackingAbility;
    public static AbstractAbility defendingAbility;

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
        CombatManager.activeChar = initiativeQueue.PopNextTurnFromTurnlist();
        CombatEventManager.TriggerEvent(CombatEventType.ON_ROUND_START);
    }

    public static void ResolveAbility(AbstractAbility ability){
        if (ability.IsUnavailable()) return;
        CombatEventManager.TriggerEvent(CombatEventType.ON_ABILITY_ACTIVATED, new CombatEventDataAbilityActivated(ability));
    }

    public static void EndRound(){

    }

    public static void CleanUp(){
        combatantDict.Clear();
    }
}