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
        set {_dieData = value; this.InitialRender();}
    } 
    public Image dieBG;
    public Image dieImage;
    public TextMeshProUGUI dieNumber;
    public TextMeshProUGUI dieRange;

    void OnEnable(){
        // CombatEventManager.Subscribe(CombatEventType.ON_DIE_ROLLED, this);
    }

    void OnDestroy(){
        // CombatEventManager.UnsubscribeAll(this);
    }

    void InitialRender(){
        dieRange.text = dieData.GetMinValue().ToString() + "-" + dieData.GetMaxValue().ToString();

        switch (dieData.dieType){
            case DieType.MELEE:
                dieBG.sprite = Resources.Load<Sprite>("Images/Attack Die");
                dieImage.sprite = Resources.Load<Sprite>("Images/Melee Die");
                break;
            case DieType.RANGED:
                dieBG.sprite = Resources.Load<Sprite>("Images/Attack Die");
                dieImage.sprite = Resources.Load<Sprite>("Images/Ranged Die");
                break;
            case DieType.BLOCK:
                dieBG.sprite = Resources.Load<Sprite>("Images/Defense Die");
                dieImage.sprite = Resources.Load<Sprite>("Images/Block Die");
                break;
            case DieType.EVADE:
                dieBG.sprite = Resources.Load<Sprite>("Images/Defense Die");
                dieImage.sprite = Resources.Load<Sprite>("Images/Evade Die");
                break;
            default:
                break;
        }
    }

    void RenderRoll(int nbr){
        dieImage.enabled = false;
        dieNumber.text = nbr.ToString();
        // StartCoroutine(DestroySelf());       // TODO: Uncomment this.
    }

    IEnumerator DestroySelf(){
        yield return new WaitForSecondsRealtime(0.5f);
        Destroy(gameObject);
    }

    // UI elements should update last
    public int GetPriority(){return 1;}

    public void HandleEvent(CombatEventData eventData){
        if (eventData.eventType == CombatEventType.ON_DIE_ROLLED){
            CombatEventDieRolled data = (CombatEventDieRolled) eventData;
            if (data.die == this.dieData){
                this.RenderRoll(data.rolledValue);
            }
        }
    }
}
