using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CombatEventManager {

    public static event Action<int> OnRoundStart;
    public static event Action<int> OnRoundEnd;
    public static event Action<AbstractCharacter> OnCharacterTurnStart;
    public static event Action<AbstractCharacter> OnCharacterTurnEnd;
    public static event Action<AbstractAbility, AbilityTargeting> OnAbilityUse;
    public static event Action<AbstractDice> OnDiceClash;
    public static event Action<AbstractAbility, AbstractDice> OnDiceClashLose;
    public static event Action<AbstractAbility, AbstractDice> OnDiceClashWin;
    public static event Action<AbstractDice> OnDicePreroll;
    public static event Action<AbstractDice> OnDiceRolled;
    public static event Action<AbstractAbility, AbstractDice, int> OnHit;
    public static event Action<AbstractAbility, AbstractAbility> OnAbilityClash;
    public static event Action<AbstractCharacter, AbstractCharacter> OnCharDeath;

    public static void InvokeAbilityUse(AbstractAbility abilityUsed, AbilityTargeting target){
        OnAbilityUse?.Invoke(abilityUsed, target);
        abilityUsed.curCooldown = abilityUsed.BASE_CD;
    }

    public static void InvokeRoundStart(int currentRound){
        OnRoundStart?.Invoke(currentRound);
    }

    public static void InvokeRoundEnd(int currentRound){
        OnRoundEnd?.Invoke(currentRound);
    }

    public static void InvokeCharTurnStart(AbstractCharacter character){
        OnCharacterTurnStart?.Invoke(character);
    }

    public static void InvokeCharTurnEnd(AbstractCharacter character){
        OnCharacterTurnEnd?.Invoke(character);
    }

    ///<summary>Called before a dice clashes with another dice.</summary>
    public static void InvokeDiceClash(AbstractDice clashingDice){
        OnDiceClash?.Invoke(clashingDice);
    }

    ///<summary>Called after a dice wins its clash.</summary>
    public static void InvokeDiceClashWin(AbstractAbility winningAbility, AbstractDice dieRolled){
        OnDiceClashWin?.Invoke(winningAbility, dieRolled);
    }

    ///<summary>Called after a dice loses its clash.</summary>
    public static void InvokeDiceClashLose(AbstractAbility losingAbility, AbstractDice dieRolled){
        OnDiceClashLose?.Invoke(losingAbility, dieRolled);
    }

    ///<summary>Called before a dice's value is rolled.</summary>
    public static void InvokeDicePreroll(AbstractDice dieRolled){
        OnDicePreroll?.Invoke(dieRolled);
    }

    ///<summary>Called after a dice's value is rolled.</summary>
    public static void InvokeDiceRolled(AbstractDice dieRolled){
        OnDiceRolled?.Invoke(dieRolled);
    }

    ///<summary>Called when an Attack or Block dice is rolled, after damage / Poise damage is dealt.</summary>
    public static void InvokeHit(AbstractAbility hittingAbility, AbstractDice dieRolled, int roll){
        OnHit?.Invoke(hittingAbility, dieRolled, roll);
    }
    
    ///<summary>Whenever this method is called, call it twice, once for each clash participant.</summary>
    public static void InvokeClashAbility(AbstractAbility clashA, AbstractAbility clashB){
        OnAbilityClash?.Invoke(clashA, clashB);
    }

    public static void InvokeCharDeath(AbstractCharacter killer, AbstractCharacter victim){
        // NOTE: killer can be NULL if killed by a status effect!
        OnCharDeath?.Invoke(killer, victim);
        // Unsubscribe the dead unit's abilities.
        // TODO: Also unsubscribe character passives
        foreach (AbstractAbility ability in victim.abilities){
            ability.Unsubscribe();
        }
        // Remove the unit CombatEventManager.positions as well as the team tracker.
        CombatManager.positions[victim.curLane].Remove(victim);
        CombatManager.factionMap[victim.CHAR_FACTION].characters.Remove(victim);

        // If the unit's faction no longer has any units and the faction is the player/enemy faction, trigger encounter end sequence.
        // Neutral factions currently have no special behavior when their last character dies.
        if (CombatManager.factionMap[victim.CHAR_FACTION].characters.Count == 0){
            if (victim.CHAR_FACTION == FactionType.PLAYER_FACTION){
                // TODO: Loss handling
            } else if (victim.CHAR_FACTION == FactionType.ENEMY_FACTION){
                // TODO: Win handling
            }
        }
    }
}