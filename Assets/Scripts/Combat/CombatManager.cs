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
    private static List<AbstractCombatAction> combatActionQueue = new List<AbstractCombatAction>();
    public static AbstractCharacter activeCharacter;

    // Tuple of (list of team members, team valor points, team type)
    public static (List<AbstractCharacter>, int, Team) PLAYER_TEAM   = (new List<AbstractCharacter>(), 0, Team.PLAYER_TEAM);
    public static (List<AbstractCharacter>, int, Team) NEUTRAL_TEAM  = (new List<AbstractCharacter>(), 0, Team.NEUTRAL_TEAM);
    public static (List<AbstractCharacter>, int, Team) ENEMY_TEAM    = (new List<AbstractCharacter>(), 0, Team.ENEMY_TEAM);
    public static readonly Dictionary<CharacterFaction, (List<AbstractCharacter>, int, Team)> mapFactionToTeam = new Dictionary<CharacterFaction, (List<AbstractCharacter>, int, Team)>{
        {CharacterFaction.PLAYER_FACTION, PLAYER_TEAM},
        {CharacterFaction.NEUTRAL_FACTION, NEUTRAL_TEAM},
        {CharacterFaction.ENEMY_FACTION, ENEMY_TEAM}
    };
    
    public static Dictionary<int, List<AbstractCharacter>> positions = new Dictionary<int, List<AbstractCharacter>>{
        {0, new List<AbstractCharacter>()},
        {1, new List<AbstractCharacter>()},
        {2, new List<AbstractCharacter>()},
        {3, new List<AbstractCharacter>()},
        {4, new List<AbstractCharacter>()},
        {5, new List<AbstractCharacter>()}
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

    public static void StartRound(){
        CombatEventManager.InvokeRoundStart(CombatManager.round);
        // At the start of a round, process start-of-round status effects, then each fighter rolls for speed equal to the number of their actions.
        // On an initiative tie, players go first, then neutrals, then enemies.
        foreach (AbstractCharacter fighter in ENEMY_TEAM.Item1.Concat(NEUTRAL_TEAM.Item1).Concat(PLAYER_TEAM.Item1).ToList()){
            for (int i = 0; i < fighter.actionsPerRound; i++){
                int speed = fighter.speedMod + Random.Range(fighter.minSpd, fighter.maxSpd + 1);
                turnQueue.Enqueue(speed, fighter);
            }
        }
        CombatManager.StartNextCharacterTurn();
    }

    public static void StartNextCharacterTurn(){
        AbstractCharacter character = turnQueue.Pop();
        if (character == null) {            // If there are no more character actions in the queue then start the next round.
            CombatManager.EndRound();
            return;
        }
        CombatManager.activeCharacter = character;
        CombatEventManager.InvokeCharTurnStart(character);
        if (character.CHAR_FACTION == CharacterFaction.ENEMY_FACTION || character.CHAR_FACTION == CharacterFaction.NEUTRAL_FACTION){
            // enemy AI processing
        } else {
            foreach (AbstractAbility ability in character.abilities){
                ability.IsActivatable();
            }
        }
    }

    public static void ActivateAbility(AbstractAbility ability, AbilityTargeting target) {
        CombatEventManager.InvokeAbilityUse(ability, target);
        
        // Utility abilities perform their work during InvokeAbilityUse so we can skip the rest of this method.
        // All code following the next line is to handle attacks.
        if (ability.ABILITY_TYPE == AbilityType.UTILITY) { return; }

        object targeting = target.GetTargeting();
        if (targeting is AbstractCharacter){    // Target is a single character.
            CombatManager.ActivateSingleTargetAbility(ability, (AbstractCharacter) targeting);
        } else {                                // Target is a list of characters, a lane, or a list of lanes.
            CombatManager.ActivateAoeAbility(ability, targeting);
        }
    }

    public static void ActivateSingleTargetAbility(AbstractAbility ability, AbstractCharacter target){
        List<AbstractDice> attackerDice = ability.GetDice();
        List<AbstractDice> defenderDice = new List<AbstractDice>();

        bool clashOccurs = CheckForClash(ability.abilityOwner, target);
        if (clashOccurs) {
            AbstractAbility targetIntent = target.currentIntent;
            CombatEventManager.InvokeAbilityUse(targetIntent, new AbilityTargeting(ability.abilityOwner));      // the defender also uses an ability!
            defenderDice = targetIntent.GetDice();

            CombatEventManager.InvokeClashAbility(ability);
            CombatEventManager.InvokeClashAbility(targetIntent);
            CombatManager.turnQueue.RemoveCharacterNextAction(target);
        }
    }

    // Abilties which target a lane, multiple lanes, or multiple characters go through this process.
    public static void ActivateAoeAbility(AbstractAbility ability, object targetingInfo){
        if (ability.ABILITY_TYPE == AbilityType.UTILITY) { return; }
        List<AbstractDice> attackerDice = ability.GetDice();
        List<AbstractCharacter> targetList = (targetingInfo is List<AbstractCharacter>) ? (List<AbstractCharacter>)targetingInfo : new List<AbstractCharacter>();

        // For lane/multi-lane targeting, get the list of all characters in those lanes.
        if (targetingInfo is int){
            targetList.AddRange(CombatManager.positions[(int)targetingInfo]);     // Prefer against Concat as that creates a copy, AddRange adds to the existing list
        } else if (targetingInfo is List<int>) {
            foreach (int lane in (List<int>)targetingInfo){
                targetList.AddRange(CombatManager.positions[lane]);
            }
        }

        while (attackerDice.Count > 0){
            AbstractDice die = attackerDice[0];
            int value = die.Roll();
            foreach (AbstractCharacter target in targetList){
                die.TriggerEffect(ability.abilityOwner, target, value);
            }
            attackerDice.RemoveAt(0);
        }
    }

    public static bool CheckForClash(AbstractCharacter attacker, AbstractCharacter defender){
        AbstractAbility attackIntent = attacker.currentIntent;
        AbstractAbility defendIntent = defender.currentIntent;
        if (attackIntent == null || defendIntent == null || !defender.HasActionsRemaining()) { return false; }
        // TODO: Add check to prevent clash if attacker intent has Sneaky tag.
        // TODO: Add check to prevent clash if defender is Stunned or Staggered.
        if (attackIntent.ABILITY_TYPE == AbilityType.UTILITY || defendIntent.ABILITY_TYPE == AbilityType.UTILITY) { return false; }
        if (attackIntent.isAoE || defendIntent.isAoE) { return false; }
        return true;
    }

    public static void EndCharacterTurn(){
        CombatEventManager.InvokeCharTurnEnd(CombatManager.activeCharacter);
        CombatManager.StartNextCharacterTurn();
    }

    public static void EndRound(){
        CombatEventManager.InvokeRoundEnd(CombatManager.round);
        CombatManager.round++;
        CombatManager.StartRound();
    }

    public static void EndCombat(bool victory){
        if (victory) {
            // Display rewards overlay
        } else {
            // If all agents are downed in combat, end the current run
        }
    }

    public static void AddActionToQueue(AbstractCombatAction action){
        CombatManager.combatActionQueue.Add(action);
    }
}