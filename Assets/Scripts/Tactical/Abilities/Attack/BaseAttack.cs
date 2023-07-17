using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : AbstractAbility {
    public static string id = "BASE_ATTACK";
    // TODO: Make all of these read in by JSON.
    private static string name = "Base Attack";
    private static string desc = "";

    // TODO: Should gameplay attributes also be defined in JSON? e.g. base CD, min range, max range, dice, etc...
    private static int cd = 1;
    private static int min_range = 0;
    private static int max_range = 0;

    public BaseAttack(): base(
        id,
        name,
        desc,
        AbilityType.ATTACK,
        cd,
        min_range,
        max_range
    ){
        Die atkDieA = new Die(DieType.MELEE, 4, 6);
        Die atkDieB = new Die(DieType.RANGED, 4, 6);
        this.BASE_DICE = new List<Die>{atkDieA, atkDieB};
    }
}