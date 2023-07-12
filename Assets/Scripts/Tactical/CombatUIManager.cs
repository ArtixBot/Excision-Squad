using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public Die testAttack;
    public Die testDefense;
    private GameObject diePrefab;

    // Start is called before the first frame update
    void Start() {
        diePrefab = Resources.Load("Prefabs/Die Prefab") as GameObject;
        testAttack = new Die(DieType.MELEE, 1, 5);
        testDefense = new Die(DieType.BLOCK, 3, 8);

        GameObject die1 = Instantiate(diePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        die1.GetComponent<DiePrefab>().dieData = testAttack;
        GameObject die2 = Instantiate(diePrefab, new Vector3(1, 0, 0), Quaternion.identity);
        die2.GetComponent<DiePrefab>().dieData = testDefense;
    }

    // // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.E)){
            int valA = testAttack.Roll();
            CombatEventManager.BroadcastEvent(new CombatEventDieRolled(testAttack, valA));
            int valD = testDefense.Roll();
            CombatEventManager.BroadcastEvent(new CombatEventDieRolled(testDefense, valD));
        }
    }
}
