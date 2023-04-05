using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUIManager : MonoBehaviour
{
    // AbstractCharacter charA;
    // AbstractAbility ability;
    // AbilityDie dieTest;
    // InitiativeQueue iq = CombatManager.initiativeQueue;

    // // Start is called before the first frame update
    // void Start() {
    //     charA = new TestCharacter();
    //     charA.actionsPerTurn = 5;
    //     charA.EquipAbility(new AttackRiposte());

    //     CombatManager.SetUp(new List<AbstractCharacter>{charA});
    //     CombatManager.StartRound();

    //     LogTurnOrder();
    // }

    // // Update is called once per frame
    // void Update() {
    //     if (Input.GetKeyUp(KeyCode.E)){
    //         CombatManager.EndTurn();
    //         LogTurnOrder();
    //     }
    // }

    // void LogTurnOrder(){
    //     Debug.Log($"Logging turn order. There are {iq.GetTurnList().Count} action(s) in the turn list. They are:");
    //     foreach((AbstractCharacter character, int spd) in iq.GetTurnList()){
    //         Debug.Log($"- {character.CHAR_NAME} (Speed: {spd})");
    //     }
    // }
}
