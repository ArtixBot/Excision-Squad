using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public enum AbilityType { MELEE, RANGED, UTILITY };

/// <summary>Ability targeting types.</summary>
public enum AbilityTargetingModifier {
    /// <summary>This modifier only affects Utility abilities. This ability targets ALL allies in a lane.</summary>
    FRIENDLY_LANE,
    /// <summary>This modifier only affects Utility abilities. This ability can only target self.</summary>
    SELF,
    /// <summary>This modifier only affects Utility abilities. This ability can target ANY ally regardless of position.</summary>
    TARGET_ALLY,
    /// <summary>This modifier only affects Utility abilities. This ability can target ANY enemy regardless of position.</summary>
    TARGET_ENEMY,
    /// <summary>This modifier only affects Utility abilities. This ability targets all allies.</summary>
    ALL_ALLIES,
    /// <summary>Has no effect on Utility abilities.
    /// If Reach is added as a condition, for melee abilities, allows them to target enemies regardless of their position.
    /// For ranged abilities, allows them to be used in the front lane.</summary>
    REACH,
    /// <summary>This ability targets ALL enemies in a lane and CANNOT be clashed. Melee abilities are still restricted to only target enemies in the frontmost lane.</summary>
    ENEMY_LANE,
    /// <summary>This ability simultaneously targets all enemies and CANNOT be clashed. If added on a Melee or Ranged ability, overrides REACH and ENEMY_LANE.</summary>
    ALL_ENEMIES,
    /// <summary>This ability simultaneously targets all allies, self, and all enemies. If added on a Melee or Ranged ability, overrides REACH, ENEMY_LANE, and ALL_ENEMIES.</summary>
    EVERYONE
};

public abstract class AbstractAbility {
    
    public readonly string      ABILITY_ID;
    public readonly string      ABILITY_NAME;
    public readonly AbilityType ABILITY_TYPE;
    public int ABILITY_CD;

    public AbstractCharacter abilityOwner;
    public List<AbstractDice> diceQueue;     // An ability consists of a list of dice, played in order.
    public List<AbilityTargetingModifier> targetingModifers = new List<AbilityTargetingModifier>();        // List of targeting condition modifiers (mostly for utility abilities but also a few AoE Melee or Ranged attacks)
    public int cooldown;                        // Current cooldown of this ability. When this ability is successfully activated, increase cooldown by ABILITY_COOLDOWN.

    public AbstractAbility(string ABILITY_ID, string ABILITY_NAME, AbilityType ABILITY_TYPE, List<AbilityTargetingModifier> targetingMods = null, int cooldown = 0){
        this.ABILITY_ID = ABILITY_ID;
        this.ABILITY_NAME = ABILITY_NAME;
        this.ABILITY_TYPE = ABILITY_TYPE;
        this.ABILITY_CD = cooldown;
        foreach (AbilityTargetingModifier mod in targetingMods){
            this.targetingModifers.Add(mod);
        }
    }
    
    /// <summary>Check if this ability can be activated. This function can be overwritten to add additional conditions to check for.</summary>
    public virtual void CheckIfActivatable(){
        if (this.cooldown > 0){
            throw new System.Exception("Cannot use this ability as it is on cooldown!");
        }
    }

    /// <summary>Return the list of units that can be targeted by this ability.</summary>
    public List<AbstractCharacter> GetTargetableUnits(){
        List<AbstractCharacter> targets = new List<AbstractCharacter>();
        if (this.ABILITY_TYPE == AbilityType.MELEE){

        } else if (this.ABILITY_TYPE == AbilityType.RANGED) { 

        } else {        // utility ability

        }
        return targets;
    }

    /// <summary>This function is run whenever an ability is selected, and returns the list of characters that can be targeted by this ability.</summary>
    public List<AbstractCharacter> ProcessSelection(){
        List<AbstractCharacter> targets = new List<AbstractCharacter>();
        try {
            this.CheckIfActivatable();
            targets = this.GetTargetableUnits();
            if (targets.Count == 0) { throw new System.Exception("Cannot use this ability as there are no valid targets!"); }
        } catch (System.Exception ex){
            Debug.Log($"Cannot use this ability, reason: {ex.Message}");
            return null;
        }
        return targets;
    }

    /// <summary>This function is run once an ability is both selected and a target is selected, but BEFORE any dice are rolled.
    public virtual void ProcessPreActivation(){}

    /// <summary>This function is run once an ability is both selected and a target is selected.
    public void ProcessActivation(){
        if (this.abilityOwner.CHAR_FACTION == CharacterFaction.ALLY){
            CombatManager.activePlayerAbility = this;
        } else {
            CombatManager.activeEnemyAbility = this;
        }
    }

    /// <summary>This function is run once an ability is both selected and a target is selected, but AFTER all dice are rolled.
    public virtual void ProcessPostActivation(){}
}