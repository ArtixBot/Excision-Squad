using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    public Die testAttack;
    public Die testDefense;
    public AbstractAbility basicAbility;
    private GameObject diePrefab;
    private GameObject abilityClashPrefab;

    // Start is called before the first frame update
    void Start() {
        diePrefab = Resources.Load("Prefabs/Die Prefab") as GameObject;
        abilityClashPrefab = Resources.Load("Prefabs/Ability Clash Prefab") as GameObject;

        testAttack = new Die(DieType.MELEE, 1, 5);
        testDefense = new Die(DieType.BLOCK, 3, 8);
        basicAbility = new BaseAttack();

        // Test die prefabs
        // GameObject die1 = Instantiate(diePrefab, new Vector3(-100, 0, 0), Quaternion.identity);
        // die1.GetComponent<DiePrefab>().dieData = testAttack;
        // die1.transform.SetParent(GameObject.Find("Clash BG").transform, false);
        // GameObject die2 = Instantiate(diePrefab, new Vector3(100, 0, 0), Quaternion.identity);
        // die2.GetComponent<DiePrefab>().dieData = testDefense;
        // die2.transform.SetParent(GameObject.Find("Clash BG").transform, false);

        // Test ability clash prefab
        GameObject ability1 = Instantiate(abilityClashPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        ability1.GetComponent<AbilityClashPrefab>().abilityData = basicAbility;
        ability1.transform.SetParent(GameObject.Find("Clash Overlay").transform, false);

        CombatManager.combatState = CombatState.COMBAT_START;
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
