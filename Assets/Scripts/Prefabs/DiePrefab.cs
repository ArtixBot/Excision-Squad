using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiePrefab : MonoBehaviour
{
    // Backing variable required, see https://stackoverflow.com/a/3276167
    private Die _dieData;
    public Die dieData{
        get => _dieData;
        set {_dieData = value; UpdateRendering();}
    } 
    public SpriteRenderer dieSprite;
    public TextMeshPro dieNumber;

    void UpdateRendering(){
        DieType dieType = _dieData.dieType;
        if (dieType == DieType.MELEE || dieType == DieType.RANGED){
            dieSprite.sprite = Resources.Load<Sprite>("Images/Attack Die");
            dieNumber.colorGradientPreset = Resources.Load<TMP_ColorGradient>("Fonts/Attack Gradient");
            dieNumber.text = "12";
        } else if (dieType == DieType.BLOCK || dieType == DieType.EVADE) {
            dieSprite.sprite = Resources.Load<Sprite>("Images/Defense Die");
            dieNumber.colorGradientPreset = Resources.Load<TMP_ColorGradient>("Fonts/Defense Gradient");
            dieNumber.text = "18";
        } else {

        }
    }

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
