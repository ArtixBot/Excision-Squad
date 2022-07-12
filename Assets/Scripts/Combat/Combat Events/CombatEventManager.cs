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
    public static event Action<AbstractAbility> OnAbilityUse;
    public static event Action<AbstractDice> OnDiceRoll;
    public static event Action<AbstractAbility, AbstractDice, int> OnHit;
    public static event Action<AbstractAbility> OnClashAbility;
    public static event Action<AbstractDice> OnClashDice;
    public static event Action<AbstractAbility, AbstractDice> OnClashWin;
    public static event Action<AbstractAbility, AbstractDice> OnClashLose;

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

    public static void InvokeAbilityUse(AbstractAbility abilityUsed){
        OnAbilityUse?.Invoke(abilityUsed);
    }

    public static void InvokeDiceRoll(AbstractDice dieRolled){
        OnDiceRoll?.Invoke(dieRolled);
    }

    public static void InvokeHit(AbstractAbility hittingAbility, AbstractDice dieRolled, int roll){
        OnHit?.Invoke(hittingAbility, dieRolled, roll);
    }
    
    ///<summary>Whenever this method is called, call it twice, once for each clash participant.</summary>
    public static void InvokeClashAbility(AbstractAbility clashingAbility){
        OnClashAbility?.Invoke(clashingAbility);
    }

    ///<summary>Whenever this method is called, call it twice, once for each die participant.</summary>
    public static void InvokeClashDice(AbstractDice clashingDice){
        OnClashDice?.Invoke(clashingDice);
    }
    
    public static void InvokeClashWin(AbstractAbility winningAbility, AbstractDice dieRolled){
        OnClashWin?.Invoke(winningAbility, dieRolled);
    }

    public static void InvokeClashLose(AbstractAbility losingAbility, AbstractDice dieRolled){
        OnClashLose?.Invoke(losingAbility, dieRolled);
    }
}