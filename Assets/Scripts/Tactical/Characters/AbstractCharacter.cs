using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterFaction {PLAYER, ALLY, NEUTRAL, ENEMY};

public abstract class AbstractCharacter {
    public CharacterFaction CHAR_FACTION;
    public string CHAR_NAME;
    public List<AbstractAbility> PERMA_ABILITIES = new List<AbstractAbility>();


    public List<AbstractAbility> abilities = new List<AbstractAbility>();       // At the start of combat, deep-copy everything from PERMA_ABILITIES.

    public int actionsPerTurn;
    public int maxHP, curHP;
    public int minSpd, maxSpd, spdMod;
    public int curPos;
    public int maxPoise, curPoise;

    // Add an ability to the character's list of equipped abilities.
    // Returns true if successful, otherwise false.
    public bool EquipAbility(AbstractAbility ability){
        if (this.PERMA_ABILITIES.Count >= 8) return false;
        // Cannot equip more than 4 generic abilities at any given time.
        if (this.PERMA_ABILITIES.FindAll(ability => ability.IS_GENERIC).Count >= 4) return false;

        ability.OWNER = this;
        this.PERMA_ABILITIES.Add(ability);
        return true;
    }

    // Remove an ability to the character's list of equipped abilities.
    // Returns true if successful, otherwise false.
    public bool UnequipAbility(AbstractAbility ability){
        return this.PERMA_ABILITIES.Remove(ability);
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
