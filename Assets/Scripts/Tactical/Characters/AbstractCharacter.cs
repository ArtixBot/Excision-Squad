using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterFaction {PLAYER, ALLY, NEUTRAL, ENEMY};

public abstract class AbstractCharacter {
    public CharacterFaction CHAR_FACTION;
    public string CHAR_NAME;

    public List<AbstractAbility> abilities = new List<AbstractAbility>();       // At the start of combat, deep-copy everything from PERMA_ABILITIES.

    public int actionsPerTurn;
    public int maxHP, curHP;
    public int minSpd, maxSpd;      // Speed modifiers like Haste or Slow are done in event handling.
    public int curPos;
    public int maxPoise, curPoise;

    // Add an ability to the character's list of equipped abilities.
    // Returns true if successful, otherwise false.
    // TODO: Move default character ability equip limits to UI logic instead? We might have in-combat granted abilities and this wouldn't work well with that.
    public bool EquipAbility(AbstractAbility ability){
        if (this.abilities.Count >= 8) return false;
        // Cannot equip more than 4 generic abilities at any given time.
        if (this.abilities.FindAll(ability => ability.IS_GENERIC).Count >= 4) return false;

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
            if (ability.isAvailable) cnt++;
        }
        return cnt;
    }

    public int CountUnavailableAbilities(){
        int cnt = 0;
        foreach (AbstractAbility ability in abilities){
            if (!ability.isAvailable) cnt++;
        }
        return cnt;
    }
}
