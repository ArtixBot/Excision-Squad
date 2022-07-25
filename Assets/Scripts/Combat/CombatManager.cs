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

    public static void ActivateAbility(AbstractAbility ability, List<AbstractCharacter> targets) {
        // AoE attacks never trigger a clash check.
        CombatEventManager.InvokeAbilityUse(ability);
    }

    public static void ActivateAbility(AbstractAbility ability, AbstractCharacter target) {
        CombatEventManager.InvokeAbilityUse(ability);
        bool clashOccurs = CheckForClash(ability.abilityOwner, target);

        List<AbstractDice> attackerDice = ability.GetDice();
        List<AbstractDice> defenderDice = new List<AbstractDice>();
        if (clashOccurs) {
            AbstractAbility targetIntent = target.currentIntent;
            CombatEventManager.InvokeAbilityUse(targetIntent);
            defenderDice = targetIntent.GetDice();

            CombatEventManager.InvokeClashAbility(ability);
            CombatEventManager.InvokeClashAbility(targetIntent);
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
}