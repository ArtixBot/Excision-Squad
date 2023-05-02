using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatEventType {
    ON_COMBAT_START, ON_COMBAT_END, // On combat start/end, an IEventObserver must subscribe/unsubscribe from all other events.
    ON_ROUND_START, ON_ROUND_END,
    ON_TURN_START, ON_TURN_END,
    ON_ABILITY_ACTIVATED,
    ON_UNIT_DEATH, ON_UNIT_DAMAGED,
    ON_DIE_ROLLED, ON_DIE_HIT,
    ON_CLASH, ON_CLASH_WIN, ON_CLASH_TIE, ON_CLASH_LOSS,
    ON_STATUS_APPLIED, ON_STATUS_EXPIRED,
}

// Custom event handler for combat events.
public static class CombatEventManager {
    public delegate void ResolveCombatStart(int round);
    static ResolveCombatStart del;
}