using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTestAttack : AbstractAbility {
    public static string ID = "TEST_ATTACK";

    public AbilityTestAttack() : base(
        ID,
        "Test Attack",
        AbilityType.MELEE
    ){}
}