using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CombatEventManager {
    private static Dictionary<CombatEvents.CombatEventType, List<CombatEventSubscriber>> eventMapping = new Dictionary<CombatEvents.CombatEventType, List<CombatEventSubscriber>>();
}