using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EncounterData {
    public List<AbstractCharacter> incomingCombatants;
}

public static class CombatManager {

    // public static Dictionary<CharacterFaction, List<AbstractCharacter>> combatantDict = new Dictionary<CharacterFaction, List<AbstractCharacter>>{
    //     {CharacterFaction.PLAYER, new List<AbstractCharacter>()},
    //     {CharacterFaction.ALLY, new List<AbstractCharacter>()},
    //     {CharacterFaction.NEUTRAL, new List<AbstractCharacter>()},
    //     {CharacterFaction.ENEMY, new List<AbstractCharacter>()}
    // };

    // // public static InitiativeQueue initiativeQueue = new InitiativeQueue();
    // public static int round;
    // public static (AbstractCharacter character, int spd) activeChar;

    // public static AbstractAbility attackingAbility;
    // public static AbstractAbility defendingAbility;

    // public static void SetUp(List<AbstractCharacter> fighters){
    //     foreach(AbstractCharacter character in fighters){
    //         combatantDict[character.CHAR_FACTION].Add(character);
    //     }
    //     CombatManager.round = 0;
    // }

    // public static void StartRound(){
    //     foreach (CharacterFaction faction in combatantDict.Keys){
    //         foreach (AbstractCharacter character in combatantDict[faction]){
    //             for (int i = 0; i < character.actionsPerTurn; i++){
    //                 // initiativeQueue.AddToQueue(character);
    //             }
    //         }
    //     }
    //     CombatManager.round += 1;
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_ROUND_START, new CombatEventDataRoundStart(CombatManager.round));

    //     // CombatManager.activeChar = initiativeQueue.GetNextTurnFromTurnlist();
    //     Debug.Log($"Start Round {CombatManager.round}!");
    // }

    // /// <summary>
    // /// Invoked when a user activates an Attack, Reaction, or Utility ability.
    // /// </summary>
    // public static void ResolveAbility(AbstractAbility ability){
    //     if (ability.IsUnavailable()) return;
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_ABILITY_ACTIVATED, new CombatEventDataAbilityActivated(ability));
    //     if (ability.ABILITY_TYPE == AbilityType.UTILITY) {return;}      // No need to go through clash/combat process if ability is Utility
    //     if (ability.ABILITY_OWNER == CombatManager.activeChar.character){
    //         CombatManager.attackingAbility = ability;
    //     } else {
    //         CombatManager.defendingAbility = ability;
    //     }
    // }

    // /// <summary>
    // /// UI should call this after:<br/>
    // /// - An Attack was used.<br/>
    // /// - The defender has had the chance to respond with another Attack or Reaction (assuming that the Attack is eligible to be clashed against).
    // /// </summary>
    // public static void ResolveCombat(){
    //     if (attackingAbility == null) {return;}

    //     int atkPtr = 0;
    //     int defPtr = 0;
    //     // While dice remain...
    //     while (atkPtr < attackingAbility.abilityDice.Count){
    //         // Roll the attacker's die.
    //         AbilityDie atkDie = attackingAbility.abilityDice[atkPtr];
    //         int atkRoll = atkDie.Roll();
    //         CombatEventManager.TriggerEvent(CombatEventType.ON_DIE_ROLLED, new CombatEventDataDieRolled(atkDie, ref atkRoll));

    //         // Roll the defender's die if applicable.
    //         AbilityDie winningDie = null;
    //         int winningRoll = -1;
    //         if (defPtr < defendingAbility.abilityDice.Count){
    //             AbilityDie defDie = defendingAbility.abilityDice[defPtr];
    //             int defRoll = defDie.Roll();
    //             CombatEventManager.TriggerEvent(CombatEventType.ON_DIE_ROLLED, new CombatEventDataDieRolled(defDie, ref defRoll));
    //             defPtr++;
    //             (winningDie, winningRoll) = CombatManager.ResolveClash(atkDie, atkRoll, defDie, defRoll);
    //         }

    //         // Check for the winning die. Uses winningDie if a clash occurred, otherwise uses atkDie (auto-win) 
    //         // CombatManager.ResolveDieEffect((winningDie != null)? winningDie : atkDie, (winningRoll != -1) ? winningRoll : atkRoll);

    //         atkPtr++;
    //     }

    //     // After combat is resolved, empty the attacking/defending ability.
    //     attackingAbility = null;
    //     defendingAbility = null;
    // }

    // public static void EndTurn(){
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_TURN_END, new CombatEventDataTurnEnd(CombatManager.activeChar.character));
    //     // CombatManager.activeChar = initiativeQueue.PopNextTurnFromTurnlist();

    //     // If there are no remaining characters in the turn list just end the round immediately.
    //     if (CombatManager.activeChar == (null, 0)) {
    //         CombatManager.EndRound();
    //         return;
    //     }

    //     CombatEventManager.TriggerEvent(CombatEventType.ON_TURN_START, new CombatEventDataTurnStart(CombatManager.activeChar.character));

    //     if (CombatManager.activeChar.character.CHAR_FACTION == CharacterFaction.ENEMY){
    //         // TODO: AI Processing, but for now just call this method again
    //         CombatManager.EndTurn();
    //     }
    // }

    // public static void EndRound(){
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_ROUND_END, new CombatEventDataRoundStart(CombatManager.round));
    //     CombatManager.StartRound();
    // }

    // public static void CleanUp(){
    //     combatantDict.Clear();
    // }

    // private static (AbilityDie winnerDie, int winnerRoll) ResolveClash(AbilityDie aDie, int aRoll, AbilityDie dDie, int dRoll){
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_CLASH, new CombatEventDataClash(aDie, ref aRoll, dDie, ref dRoll));
    //     if (aRoll == dRoll) {
    //         CombatEventManager.TriggerEvent(CombatEventType.ON_CLASH_TIE, new CombatEventDataClashTie());
    //         return (null, aRoll);
    //     }
    //     AbilityDie winnerDie = (aRoll > dRoll) ? aDie : dDie;
    //     AbilityDie loserDie = (winnerDie == aDie) ? dDie : aDie;
    //     int winnerRoll = (winnerDie == aDie) ? aRoll : dRoll;
    //     int loserRoll  = (winnerDie == aDie) ? dRoll : aRoll;
        
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_CLASH_WIN, new CombatEventDataClashWin(winnerDie, ref winnerRoll, loserDie, ref loserRoll));
    //     CombatEventManager.TriggerEvent(CombatEventType.ON_CLASH_LOSS, new CombatEventDataClashLoss(winnerDie, ref winnerRoll, loserDie, ref loserRoll));
    //     return (winnerDie, winnerRoll);
    // }

    // private static void ResolveDieEffect(AbilityDie die, int dieValue){
    //     switch (die.GetDieType()){
    //         default:
    //             break;
    //     }
    // }
}