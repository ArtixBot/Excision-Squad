using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatManager {

    public static int round = 1;
    public static InitiativeQueue turnQueue = new InitiativeQueue();
    public static List<AbstractCharacter> battleParticipants = new List<AbstractCharacter>();

    public static AbstractAbility activePlayerAbility;
    public static AbstractAbility activeEnemyAbility;

    public static void StartRound(){

        // At the start of a round, each fighter rolls for speed equal to the number of their actions.
        // TODO: Because of how InitiatveQueue works, have enemies roll for speed first and then players (so that in the case of a speed tie, the player always gets to go first.)
        foreach (AbstractCharacter fighter in battleParticipants){
            for (int i = 0; i < fighter.actionsPerRound; i++){
                int speed = fighter.speedMod + Random.Range(fighter.minSpeed, fighter.maxSpeed + 1);
                turnQueue.Enqueue(speed, fighter);
            }
        }
    }

    public static void NextCharacter(){
        AbstractCharacter character = turnQueue.Pop();
        if (character.CHAR_FACTION == CharacterFaction.ENEMY){
            // enemy AI processing
        } else {
            // player takes turn
        }
    }

    public static void StartCombat(List<AbstractCharacter> fighters){
        CombatManager.round = 1;
        CombatManager.battleParticipants = fighters;

        CombatManager.StartRound();
    }

    public static void EndCombat(bool playerVictorious){
        if (playerVictorious) {
            // Display rewards overlay
        } else {
            // If all agents are downed in combat, end the current run
        }
    }

    // public static void ProcessAbility(AbstractAbility ability, List<AbstractCharacter> targets){
    //     // Single-target ability. If this is not a Utility ability, check to see if the enemy has an available action, and if they do, give them the option to clash against this ability
    //     if (targets.Count == 1 && (ability.ABILITY_TYPE == AbilityType.MELEE || ability.ABILITY_TYPE == AbilityType.RANGED)){
    //         AbstractCharacter target = targets[0];
    //         if (target.HasActionsRemaining()){
    //             // allow clash selection
    //         }
    //     }
    //     else {  // AoE/Utility ability, which cannot be clashed against.

    //     }
    // }

    /// <summary>Invoked by ResolveCombat. Runs through multiple conditions to see if a clash should be initiated when ResolveCombat is run.</summary>
    private static bool CheckForClash(){
        AbstractAbility playerAbility = CombatManager.activePlayerAbility;
        AbstractAbility enemyAbility = CombatManager.activeEnemyAbility;

        // No clash if either the player/enemy ability isn't active
        if (playerAbility == null || enemyAbility == null) return false;
        // No clash if either the active player/enemy ability is a utility ability
        if (playerAbility.ABILITY_TYPE == AbilityType.UTILITY || enemyAbility.ABILITY_TYPE == AbilityType.UTILITY) return false;
        // No clash if either the active player/enemy ability is an AoE attack
        if (playerAbility.targetingModifers.Contains(AbilityTargetingModifier.ENEMY_LANE) || enemyAbility.targetingModifers.Contains(AbilityTargetingModifier.ENEMY_LANE)) return false;
        if (playerAbility.targetingModifers.Contains(AbilityTargetingModifier.ALL_ENEMIES) || enemyAbility.targetingModifers.Contains(AbilityTargetingModifier.ALL_ENEMIES)) return false;
        if (playerAbility.targetingModifers.Contains(AbilityTargetingModifier.EVERYONE) || enemyAbility.targetingModifers.Contains(AbilityTargetingModifier.EVERYONE)) return false;
        return true;
    }

    /// <summary>Given two dice, roll them and then return the winning dice as well as the value that dice rolled.</summary>.
    private static (AbstractDice, int) ResolveClash(AbstractDice a, AbstractDice b){
        int aVal = a.Roll(), bVal = b.Roll();

        if (aVal == bVal){ return (null, 0); }      // Ties always have no winner

        if (a.GetType() == DiceType.ATTACK && b.GetType() == DiceType.BLOCK || 
            a.GetType() == DiceType.BLOCK && b.GetType() == DiceType.ATTACK){
            // Mitigation scenario (Attack v. Block)
            // Mitigation scenario: the higher roll wins but its final value is reduced by the smaller roll.
            return (aVal > bVal) ? (a, aVal - bVal) : (b, bVal - aVal);        
        } else {
            // All-or-nothing scenario (Attack v. Evade, Attack v. Attack, Evade v. Evade, Block v. Evade, Block v. Block).
            // All-or-nothing scenarios: the higher roll wins and its final value is equal to the higher roll.
            return (aVal > bVal) ? (a, aVal) : (b, bVal);
        }
    }

    /// <summary>This function will automatically process the active player and active enemy ability, and then returns a list of the resulting combat results (to be sent to CombatRender)</summary>
    public static void ResolveCombat(){
        List<AbstractDice> pDiceQueue = activePlayerAbility.GetDice(), eDiceQueue = activeEnemyAbility.GetDice();

        // HANDLE CLASHES
        if (CheckForClash()){
            while (pDiceQueue.Count > 0 && eDiceQueue.Count > 0){
                AbstractDice pDice = pDiceQueue[0], eDice = eDiceQueue[0];
                // TODO: Run OnClash events for pDice and eDice.
                (AbstractDice, int) clashResult = CombatManager.ResolveClash(pDice, eDice);
                if (clashResult.Item1 != null){
                    // TODO: Add OnClashWin/OnClashLose events for winner dice/loser dice
                    AbstractDice winner = clashResult.Item1;
                    switch (winner.GetType()){
                        case DiceType.ATTACK:
                            // add damage action to combat queue
                            break;
                        case DiceType.BLOCK:
                            // add poise damage action to combat queue
                            break;
                        case DiceType.EVADE:
                            // add recover poise action to combat queue; if against an attack, add the dice back to the queue
                            break;
                    }
                }
                
                pDiceQueue.RemoveAt(0);
                eDiceQueue.RemoveAt(0);
            }
        }
        // HANDLE ONE-SIDED ATTACKS.
        while (pDiceQueue.Count > 0){
            AbstractDice pDice = pDiceQueue[0];
            int roll = pDice.Roll();
            switch (pDice.GetType()){
                case DiceType.ATTACK:
                    // add damage action to combat queue
                    break;
                case DiceType.BLOCK:
                case DiceType.EVADE:
                    // for now, no effect for a one-sided block/evade
                    break;
            }
            pDiceQueue.RemoveAt(0);
        }
        while (eDiceQueue.Count > 0){
            AbstractDice eDice = eDiceQueue[0];
            int roll = eDice.Roll();
            switch (eDice.GetType()){
                case DiceType.ATTACK:
                    // add damage action to combat queue
                    break;
                case DiceType.BLOCK:
                case DiceType.EVADE:
                    // for now, no effect for a one-sided block/evade
                    break;
            }
            eDiceQueue.RemoveAt(0);
        }
        
        CombatManager.activePlayerAbility = null;
        CombatManager.activeEnemyAbility = null;
        return;
    }
}