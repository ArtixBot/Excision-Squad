using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* Combat Order:
 * Before Round Start (resolve all status effects)
 * Round Start  (roll for initiative)
 * Players/Enemies play their turns
 * Round End
*/
public enum Team {PLAYER_TEAM, NEUTRAL_TEAM, ENEMY_TEAM};

public static class CombatManager {

    public static CombatRender combatRender;


    // =========== COMBAT STATE VARIABLES ===========
    public static int round = 1;
    public static InitiativeQueue turnQueue = new InitiativeQueue();
    public static List<AbstractCombatAction> combatActionQueue = new List<AbstractCombatAction>();

    // Tuple of (list of team members, team valor points, team type)
    public static (List<AbstractCharacter>, int, Team) PLAYER_TEAM   = (new List<AbstractCharacter>(), 0, Team.PLAYER_TEAM);
    public static (List<AbstractCharacter>, int, Team) NEUTRAL_TEAM  = (new List<AbstractCharacter>(), 0, Team.NEUTRAL_TEAM);
    public static (List<AbstractCharacter>, int, Team) ENEMY_TEAM    = (new List<AbstractCharacter>(), 0, Team.ENEMY_TEAM);
    public static readonly Dictionary<CharacterFaction, (List<AbstractCharacter>, int, Team)> mapFactionToTeam = new Dictionary<CharacterFaction, (List<AbstractCharacter>, int, Team)>{
        {CharacterFaction.PLAYER_FACTION, PLAYER_TEAM},
        {CharacterFaction.NEUTRAL_FACTION, NEUTRAL_TEAM},
        {CharacterFaction.ENEMY_FACTION, ENEMY_TEAM}
    };
    // ========= END COMBAT STATE VARIABLES ========

    public static void StartCombat(List<AbstractCharacter> fighters){
        combatRender = GameObject.FindObjectOfType<CombatRender>();
        CombatManager.round = 1;
        foreach (AbstractCharacter fighter in fighters){
            mapFactionToTeam[fighter.CHAR_FACTION].Item1.Add(fighter);
        }
        CombatManager.StartRound();
    }

    public static void EndCombat(bool victory){
        if (victory) {
            // Display rewards overlay
        } else {
            // If all agents are downed in combat, end the current run
        }
    }

    public static void StartRound(){
        // At the start of a round, process start-of-round status effects, then each fighter rolls for speed equal to the number of their actions.
        // On an initiative tie, players go first, then neutrals, then enemies.
        foreach (AbstractCharacter fighter in ENEMY_TEAM.Item1.Concat(NEUTRAL_TEAM.Item1).Concat(PLAYER_TEAM.Item1).ToList()){
            for (int i = 0; i < fighter.actionsPerRound; i++){
                int speed = fighter.speedMod + Random.Range(fighter.minSpd, fighter.maxSpd + 1);
                turnQueue.Enqueue(speed, fighter);
            }
        }
        CombatManager.NextCharacter();
    }

    public static void NextCharacter(){
        AbstractCharacter character = turnQueue.Pop();
        if (character.CHAR_FACTION == CharacterFaction.ENEMY_FACTION || character.CHAR_FACTION == CharacterFaction.NEUTRAL_FACTION){
            // enemy AI processing
        } else {
            // foreach (AbstractAbility ability in character.abilities){
            //     ability.IsActivatable();
            // }
        }
    }
    
    public static List<AbstractCharacter> calculateEligibleTargets(AbstractAbility ability){
        List<AbstractCharacter> targets = new List<AbstractCharacter>();
        AbstractCharacter initiator = ability.abilityOwner;
        return targets;
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

    /// AoE abilities never can be clashed against, but this is here just for consistency.
    private static bool CheckForClash(AbstractAbility initiator, List<AbstractCharacter> targets){ return false; }
    /// <summary>Invoked by ResolveCombat. Runs through multiple conditions to see if a clash should be initiated when ResolveCombat is run.</summary>
    private static bool CheckForClash(AbstractAbility initiator, AbstractCharacter target){
        if (initiator.isAoE || initiator.ABILITY_TYPE == AbilityType.UTILITY) return false;
        
        AbstractAbility targetIntent = target.currentIntent;
        if (targetIntent == null || targetIntent.ABILITY_TYPE == AbilityType.UTILITY || targetIntent.isAoE) return false;
        return true;
    }

    /// <summary>Given two dice, roll them and then return (winning dice, value winning dice rolled, losing dice).</summary>.
    private static (AbstractDice, int, AbstractDice) ResolveClash(AbstractDice a, AbstractDice b){
        int aVal = a.Roll(), bVal = b.Roll();

        if (aVal == bVal){ return (null, 0, null); }      // Ties always have no winner

        if (a.GetType() == DiceType.ATTACK && b.GetType() == DiceType.BLOCK || 
            a.GetType() == DiceType.BLOCK && b.GetType() == DiceType.ATTACK){
            // Mitigation scenario (Attack v. Block)
            // Mitigation scenario: the higher roll wins but its final value is reduced by the smaller roll.
            return (aVal > bVal) ? (a, aVal - bVal, b) : (b, bVal - aVal, a);        
        } else {
            // All-or-nothing scenario (Attack v. Evade, Attack v. Attack, Evade v. Evade, Block v. Evade, Block v. Block).
            // All-or-nothing scenarios: the higher roll wins and its final value is equal to the higher roll.
            return (aVal > bVal) ? (a, aVal, b) : (b, bVal, a);
        }
    }

    /// <summary>This function will automatically process the active player and active enemy ability, and then returns a list of the resulting combat results (to be sent to CombatRender)</summary>
    public static void ResolveCombat(AbstractCharacter attacker, AbstractCharacter target, AbstractAbility attackerAbility, AbstractAbility defenderAbility){

        // // null checks
        // List<AbstractDice> atkDiceQueue = attackerAbility?.GetDice() ?? new List<AbstractDice>{};
        // List<AbstractDice> defDiceQueue = defenderAbility?.GetDice() ?? new List<AbstractDice>{};

        // // HANDLE CLASHES
        // if (CheckForClash()){
        //     while (atkDiceQueue.Count > 0 && defDiceQueue.Count > 0){
        //         AbstractDice pDice = atkDiceQueue[0], eDice = defDiceQueue[0];
        //         // TODO: Run OnClash events for pDice and eDice.
        //         (AbstractDice, int, AbstractDice) clashResult = CombatManager.ResolveClash(pDice, eDice);
        //         if (clashResult.Item1 != null){
        //             // TODO: Add OnClashWin/OnClashLose events for winner dice/loser dice
        //             AbstractDice winner = clashResult.Item1;
        //             AbstractDice loser = clashResult.Item3;
        //             switch (winner.GetType()){
        //                 case DiceType.ATTACK:
        //                     combatActionQueue.Add(new CombatActionAttack(winner.diceOwner, loser.diceOwner, clashResult.Item2));
        //                     break;
        //                 case DiceType.BLOCK:
        //                     combatActionQueue.Add(new CombatActionBlock(loser.diceOwner, clashResult.Item2));
        //                     break;
        //                 case DiceType.EVADE:
        //                     combatActionQueue.Add(new CombatActionEvade(winner.diceOwner, clashResult.Item2));
                            
        //                     // When evading an attack, evade dice gets to be rerolled.
        //                     if (loser.GetType() == DiceType.ATTACK){
        //                         if (winner == pDice){
        //                             atkDiceQueue.Add(pDice);
        //                         } else {
        //                             defDiceQueue.Add(eDice);
        //                         }
        //                     }
        //                     break;
        //             }
        //         }
                
        //         atkDiceQueue.RemoveAt(0);
        //         defDiceQueue.RemoveAt(0);
        //     }
        //     // Handle any leftover dice from the clash
        //     while (atkDiceQueue.Count > 0){
        //         AbstractDice pDice = atkDiceQueue[0];
        //         int roll = pDice.Roll();
        //         switch (pDice.GetType()){
        //             case DiceType.ATTACK:
        //                 combatActionQueue.Add(new CombatActionAttack(pDice.diceOwner, target, roll));
        //                 break;
        //             case DiceType.BLOCK:        // for now, no effect on a one-sided block/evade
        //             case DiceType.EVADE:
        //                 break;
        //         }
        //         atkDiceQueue.RemoveAt(0);
        //     }
        //     while (defDiceQueue.Count > 0){
        //         AbstractDice eDice = defDiceQueue[0];
        //         int roll = eDice.Roll();
        //         switch (eDice.GetType()){
        //             case DiceType.ATTACK:
        //                 combatActionQueue.Add(new CombatActionAttack(eDice.diceOwner, target, roll));
        //                 break;
        //             case DiceType.BLOCK:
        //             case DiceType.EVADE:
        //                 break;
        //         }
        //         defDiceQueue.RemoveAt(0);
        //     }
        // } else {
        //     // ONE-SIDED attack; just roll the attacker dice
        //     while (atkDiceQueue.Count > 0){
        //         AbstractDice pDice = atkDiceQueue[0];
        //         int roll = pDice.Roll();
        //         switch (pDice.GetType()){
        //             case DiceType.ATTACK:
        //                 combatActionQueue.Add(new CombatActionAttack(pDice.diceOwner, target, roll));
        //                 break;
        //             case DiceType.BLOCK:
        //             case DiceType.EVADE:
        //                 break;
        //         }
        //         atkDiceQueue.RemoveAt(0);
        //     }
        // }
        
        // CombatManager.activePlayerAbility = null;
        // CombatManager.activeEnemyAbility = null;
        // return;
    }
}