using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitiativeQueue {
    private List<(AbstractCharacter character, int spd)> turnList = new List<(AbstractCharacter character, int spd)>();

    public void Clear(){
        this.turnList.Clear();
    }

    /// <summary>
    /// Given an AbstractCharacter, roll a speed value from [character.minSpd, character.maxSpd] and place that character in the appropriate queue position based on the rolled speed.
    /// </summary>
    public void AddCombatantToQueue(AbstractCharacter character){
        int spd = Random.Range(character.minSpd, character.maxSpd) + character.spdMod;
        int i = 0;
        while (i < this.turnList.Count){
            // If the rolled speed is <= the speed at the current index, move to the next index, otherwise break and insert character into current index.
            if (spd <= this.turnList[i].spd){
                i++;
            } else {
                break;
            }
        }
        this.turnList.Insert(i, (character, spd));
    }

    /// <summary>Returns the next character in the initaitive queue in the tuple (AbstractCharacter character, int spd).<br/>
    /// If no one remains in the queue, return (null, 0) instead.</summary>
    public (AbstractCharacter character, int spd) GetNextTurnFromTurnlist(){
        if (this.turnList == null || this.turnList.Count <= 0) return (null, 0);
        (AbstractCharacter character, int spd) nextChar = this.turnList[0];
        return nextChar;
    }

    /// <summary>Returns the next character in the initaitive queue in the tuple (AbstractCharacter character, int spd).<br/>
    /// Remove the character from the front of the turnlist afterwards.<br/>
    /// If no one remains in the queue, return (null, 0) instead.</summary>
    public (AbstractCharacter character, int spd) PopNextTurnFromTurnlist(){
        if (this.turnList == null || this.turnList.Count <= 0) return (null, 0);
        (AbstractCharacter character, int spd) nextChar = GetNextTurnFromTurnlist();
        this.turnList.RemoveAt(0);
        return nextChar;
    }

    /// <summary>Find and return the next action of a character in the initaitive queue in the tuple (AbstractCharacter character, int spd).<br/>
    /// If that character does not have any remaining actions in the queue, return (null, 0) instead.</summary>
    public (AbstractCharacter character, int spd) GetNextCharacterTurn(AbstractCharacter characterToFind){
        foreach((AbstractCharacter character, int spd) pair in this.turnList){
            if (pair.character.Equals(characterToFind)){
                return pair;
            }
        }
        return (null, 0);
    }

    /// <summary>Return true if the character is in the initiative queue, false otherwise.</summary>
    public bool ContainsCharacter(AbstractCharacter character){
        foreach((AbstractCharacter character, int spd) pair in this.turnList){
            if (pair.character.Equals(character)){
                return true;
            }
        }
        return false;
    }

    /// <summary>Removes the next action of a specific character from the queue (use when a character clashes).</summary>
    public void RemoveNextCharacterAction(AbstractCharacter character){
        int i = 0;
        while (i < this.turnList.Count) {
            if (this.turnList[i].character == character){
                this.turnList.RemoveAt(i);
                return;
            }
            i++;
        }
        return;
    }

    /// <summary>Removes all of a character's actions from the queue (use when a character is staggered, killed, stunned, etc.).</summary>
    public void RemoveCharacterFromQueue(AbstractCharacter character){
        this.turnList.RemoveAll(item => item.character.Equals(character));
    }
    
    public List<(AbstractCharacter character, int spd)> GetTurnList(){
        return this.turnList;
    }
}
