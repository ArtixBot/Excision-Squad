using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Essentially a wrapper of a List<>, intended to "emulate" a priority queue.
public class InitiativeQueue {
    private List<(int, AbstractCharacter)> turnList = new List<(int, AbstractCharacter)>();

    public void Clear(){
        this.turnList.Clear();
    }

    public void Enqueue(int speed, AbstractCharacter character){
        int i = 0;
        while (i < this.turnList.Count){
            if (speed < this.turnList[i].Item1){
                i++;
            } else {        // The turnlist is in descending order of speed; if our current order of speed is >= the speed at index i, stop immediately.
                break;
            }
        }
        this.turnList.Insert(i, (speed, character));
    }

    public AbstractCharacter Pop(){
        if (this.turnList.Count == 0) { return null; }
        AbstractCharacter character = this.turnList[0].Item2;
        this.turnList.RemoveAt(0);
        return character;
    }

    public AbstractCharacter Get(){
        if (this.turnList.Count == 0) { return null; }
        AbstractCharacter character = this.turnList[0].Item2;
        return character;
    }
    
    public bool ContainsCharacter(AbstractCharacter character){
        foreach((int, AbstractCharacter) pair in this.turnList){
            if (pair.Item2.Equals(character)){
                return true;
            }
        }
        return false;
    }

    public List<(int, AbstractCharacter)> GetTurnList(){
        return this.turnList;
    }

    // <summary>Removes the next action of a specific character from the queue (use when a character clashes)</summary>
    public void RemoveCharacterNextAction(AbstractCharacter character){
        if (this.ContainsCharacter(character)){
            (int, AbstractCharacter) nextCharAction = this.turnList.First(i => i.Item2.Equals(character));
            this.turnList.Remove(nextCharAction);
        }
    }

    // <summary>Remove all actions of a specific character from the queue (use when a character dies or has Poise broken)</summary>
    public void RemoveCharacter(AbstractCharacter character){
        this.turnList.RemoveAll(item => item.Item2.Equals(character));
    }
}