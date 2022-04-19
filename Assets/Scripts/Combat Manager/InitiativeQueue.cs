using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Essentially a wrapper of a List<>, intended to "emulate" a priority queue.
public class InitiativeQueue {
    private List<(int, AbstractCharacter)> turnList = new List<(int, AbstractCharacter)>();

    public void Clear(){
        turnList.Clear();
    }

    public void Enqueue(int speed, AbstractCharacter character){
        int i = 0;
        while (i < turnList.Count){
            if (speed < turnList[i].Item1){
                i++;
            }
        }
        turnList.Insert(i, (speed, character));
    }

    public AbstractCharacter PopNextCharacter(){
        if (turnList.Count == 0) { return null; }
        AbstractCharacter character = turnList[0].Item2;
        turnList.RemoveAt(0);
        return character;
    }

    public AbstractCharacter GetNextCharacter(){
        if (turnList.Count == 0) { return null; }
        AbstractCharacter character = turnList[0].Item2;
        return character;
    }

    public List<(int, AbstractCharacter)> GetTurnList(){
        return turnList;
    }

    // <summary>Remove all actions of a specific character from the queue (use when a character dies or has Poise broken)</summary>
    public void RemoveCharacter(AbstractCharacter character){
        turnList.RemoveAll(item => item.Equals(character));
    }
}