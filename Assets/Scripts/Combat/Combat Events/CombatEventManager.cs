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
    public static event Action<AbstractAbility> OnClashAbility;
    public static event Action<AbstractCharacter, AbstractCharacter> OnCharDeath;

    public static void InvokeAbilityUse(AbstractAbility abilityUsed, AbilityTargeting target){
        OnAbilityUse?.Invoke(abilityUsed, target);
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
    public static void InvokeClashAbility(AbstractAbility clashingAbility){
        OnClashAbility?.Invoke(clashingAbility);
    }

    public static void InvokeCharDeath(AbstractCharacter killer, AbstractCharacter victim){
        // NOTE: killer can be NULL if killed by a status effect!
        OnCharDeath?.Invoke(killer, victim);
    }
}