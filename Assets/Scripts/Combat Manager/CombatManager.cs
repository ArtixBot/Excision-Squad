using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatManager {

    public static int round = 1;
    public static InitiativeQueue turnQueue = new InitiativeQueue();
    public static List<AbstractCharacter> battleParticipants = new List<AbstractCharacter>();

    public static void StartRound(){

        // At the start of a round, each fighter rolls for speed equal to the number of their actions.
        // TODO: Because of how InitiatveQueue works, have enemies roll for speed first and then players (so that in the case of a speed tie, the player always gets to go first.)
        foreach (AbstractCharacter fighter in battleParticipants){
            for (int i = 0; i < fighter.actionsPerRound; i++){
                int speed = fighter.speedMod + Random.Range(fighter.minSpeed, fighter.maxSpeed + 1);
                turnQueue.Enqueue(speed, fighter);
            }
        }
    }
}
