using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMove : AbstractAbility {
    public static string ID = "MOVE";

    public AbilityMove() : base(
        ID,
        "Move",
        AbilityType.UTILITY,
        new List<AbilityTargetingModifier>{AbilityTargetingModifier.FRIENDLY_LANE}
    ){}
}