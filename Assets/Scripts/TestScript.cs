using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    void Start()
    {
        DiceAttack test = new DiceAttack(4, 8);
        for (int i = 0; i < 100; i++){
            Debug.Log(test.Roll());
        }
    }

}
