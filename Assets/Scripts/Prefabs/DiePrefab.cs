using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiePrefab : MonoBehaviour, IEventSubscriber
{
    // Backing variable required, see https://stackoverflow.com/a/3276167
    private Die _dieData;
    public Die dieData{
        get => _dieData;
        set {_dieData = value; this.RenderBackground();}
    } 
    public SpriteRenderer dieSprite;
    public TextMeshPro dieNumber;

    void OnEnable(){
        CombatEventManager.Subscribe(CombatEventType.ON_DIE_ROLLED, this);
    }

    void OnDestroy(){
        CombatEventManager.UnsubscribeAll(this);
    }

    void RenderBackground(){
        DieType dieType = dieData.dieType;
        if (dieType == DieType.MELEE || dieType == DieType.RANGED){
            dieSprite.sprite = Resources.Load<Sprite>("Images/Attack Die");
            dieNumber.colorGradientPreset = Resources.Load<TMP_ColorGradient>("Fonts/Attack Gradient");
        } else if (dieType == DieType.BLOCK || dieType == DieType.EVADE) {
            dieSprite.sprite = Resources.Load<Sprite>("Images/Defense Die");
            dieNumber.colorGradientPreset = Resources.Load<TMP_ColorGradient>("Fonts/Defense Gradient");
        }
    }

    void RenderText(int nbr){
        dieNumber.text = nbr.ToString();
    }

    // UI elements should update last
    public int GetPriority(){return 1;}

    public void HandleEvent(CombatEventData eventData){
        if (eventData.eventType == CombatEventType.ON_DIE_ROLLED){
            CombatEventDieRolled data = (CombatEventDieRolled) eventData;
            if (data.die == this.dieData){
                this.RenderText(data.rolledValue);
            }
        }
    }
}
