using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterFaction {PLAYER, ALLY, NEUTRAL, ENEMY};

public abstract class AbstractCharacter {
    public CharacterFaction CHAR_FACTION;
    public string CHAR_NAME;
    public List<AbstractAbility> abilities = new List<AbstractAbility>();

    public int actionsPerTurn;
    public int maxHP, curHP;
    public int minSpd, maxSpd, spdMod;
    public int curPos;
    public int maxPoise, curPoise;

    // Add an ability to the character's list of equipped abilities.
    // Returns true if successful, otherwise false.
    public bool EquipAbility(AbstractAbility ability){
        if (abilities.Count >= 8) return false;

        ability.OWNER = this;
        this.abilities.Add(ability);
        return true;
    }

    // Remove an ability to the character's list of equipped abilities.
    // Returns true if successful, otherwise false.
    public bool UnequipAbility(AbstractAbility ability){
        return this.abilities.Remove(ability);
    }

    public int CountAvailableAbilities(){
        int cnt = 0;
        foreach (AbstractAbility ability in abilities){
            if (ability.IsAvailable()) cnt++;
        }
        return cnt;
    }

    public int CountUnavailableAbilities(){
        int cnt = 0;
        foreach (AbstractAbility ability in abilities){
            if (ability.IsUnavailable()) cnt++;
        }
        return cnt;
    }
}
