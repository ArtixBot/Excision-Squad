using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    AbilityDie dieTest;
    // Start is called before the first frame update
    void Start() {
        dieTest = new AbilityDie(DieType.MELEE, 4, 7);
        Debug.Log($"{dieTest.GetDieType()} {dieTest.GetMinValue()}-{dieTest.GetMaxValue()}");
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(dieTest.Roll());
    }
}
