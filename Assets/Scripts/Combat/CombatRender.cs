using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRender : MonoBehaviour
{
    public Queue<IEnumerator> animationQueue = new Queue<IEnumerator>();

    void Start() {
        List<AbstractCharacter> testData = new List<AbstractCharacter>();
        testData.Add(new CharacterDeckard());
        testData.Add(new CharacterDeckard());
        testData[1].CHAR_NAME = "Deckard 2";
        CombatManager.StartCombat(testData);
        // CombatManager.activePlayerAbility = new AbilityTestAttack();
        // CombatManager.ResolveCombat();

        // DiceAttack test = new DiceAttack(4, 8);
        // for (int i = 0; i < 100; i++){
        //     Debug.Log(test.Roll(0, -8));
        // }

        // AbstractAbility testAttack = new AbilityTestAttack();
        // foreach (AbstractDice dice in testAttack.GetDice()){
        //     Debug.Log($"Attack: {dice.Roll()}");
        // }

        // animationQueue.Enqueue(PlayAnimation());
        // animationQueue.Enqueue(PlayAnimation());
        // animationQueue.Enqueue(PlayAnimation());
        // animationQueue.Enqueue(PlayAnimation());
        // StartCoroutine(CoroutineController());
        // CombatManager.StartCombat(new List<AbstractCharacter>{new CharacterDeckard(), new CharacterDeckard()});
        // Debug.Log(CombatManager.battleParticipants.Count);
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.E)){
            Debug.Log("E");
        }
        if (Input.GetKeyUp(KeyCode.Q)){
            string debug = "Currently round {0}. Current active character: {1}. Current turn order: ";
            // foreach ((int, AbstractCharacter) character in CombatManager.turnQueue.GetTurnList()){
            //     debug = debug + "\n - " + character.Item2.CHAR_NAME;
            // }
            Debug.LogFormat(debug, CombatManager.round, CombatManager.activeCharacter.CHAR_NAME);
        }
    }

    // private IEnumerator CoroutineController(){
    //     while (true){
    //         while (animationQueue.Count > 0){
    //             yield return StartCoroutine(animationQueue.Dequeue());
    //         }
    //         yield return null;
    //     }
    // }

    // public static IEnumerator PlayAnimation(string animId = "nothing", float animationTime = 1.0f){
    //     Debug.Log($"Playing attack animation, damage dealt: {Random.Range(1, 100)}");
    //     yield return new WaitForSeconds(animationTime);
    // }

}
