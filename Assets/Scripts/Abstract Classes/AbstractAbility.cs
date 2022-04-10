using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public enum AbilityType { MELEE, RANGED, UTILITY };

/// <summary>Ability targeting types.</summary>
public enum AbilityTargetingType {
    /// <summary>Standard melee targeting; this ability can only target enemies that are in the closest lane.</summary>
    MELEE, 
    /// <summary>Standard ranged targeting; this ability can target any enemy but is unusable in the frontmost lane.</summary>         
    RANGED,
    /// <summary>This ability can only target self.</summary>
    SELF,
    /// <summary>This ability will target ALL allies in a lane.</summary>
    FRIENDLY_LANE,
    /// <summary>This ability will target ALL enemies in a lane.</summary>
    ENEMY_LANE,
    /// <summary>This ability can target ANY ally regardless of position.</summary>
    TARGET_ALLY,
    /// <summary>This ability can target ANY enemy regardless of position.</summary>
    TARGET_ENEMY,
    /// <summary>This ability targets all allies.</summary>
    ALL_ALLIES,
    /// <summary>This ability targets all enemies.</summary>
    ALL_ENEMIES,
    /// <summary>This ability targets all allies, self, and all enemies.</summary>
    EVERYONE
};

public abstract class AbstractAbility {
    
    public readonly string      ABILITY_ID;
    public readonly string      ABILITY_NAME;
    public readonly AbilityType ABILITY_TYPE;
    public int ABILITY_COOLDOWN;

    public ListDictionary abilityQueue;     // An ability consists of a list of dice, played in order. Each of those dice may have a list of associated effects (on hit, on clash win, on clash lose, etc.)
    public AbilityTargetingType targeting;
    public int cooldown;                    // When this ability is successfully activated, increase cooldown by ABILITY_COOLDOWN.
    
    // Check if this ability can be activated. This function can be overwritten to add additional conditions to check for.
    public virtual void CheckIfActivatable(){
        if (this.cooldown > 0){
            throw new System.Exception("Cannot use this ability as it is on cooldown!");
        }
    }
}