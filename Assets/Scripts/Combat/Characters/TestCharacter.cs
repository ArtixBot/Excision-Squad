using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : AbstractCharacter
{
    public TestCharacter(){
        this.CHAR_FACTION = CharacterFaction.PLAYER;
        this.CHAR_NAME = "Test Character";
        this.actionsPerTurn = 2;
        this.maxHP = this.curHP = 20;
        (this.minSpd, this.maxSpd, this.spdMod) = (2, 5, 0);
        this.curPos = 0;
        this.maxPoise = this.curPoise = 20;
    }
}
