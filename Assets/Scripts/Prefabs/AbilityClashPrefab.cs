using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityClashPrefab : MonoBehaviour, IEventSubscriber
{
    // Backing variable required, see https://stackoverflow.com/a/3276167
    private AbstractAbility _abilityData;
    public AbstractAbility abilityData{
        get => _abilityData;
        set {_abilityData = value; this.InitialRender();}
    }

    [SerializeField]
    private GameObject diePrefab;

    public TextMeshProUGUI abilityTitle;
    public GameObject dieHolder;

    public bool renderReverse = false;      // Since a clash uses two of these prefabs, need to have an easy render method to flip how it's rendered.

    void InitialRender(){
        abilityTitle.text = abilityData.NAME;
        for (int i = 0; i < abilityData.BASE_DICE.Count; i++){
            GameObject die = GameObject.Instantiate(diePrefab, new Vector3(-100 * i, 0, 0), Quaternion.identity);
            die.transform.SetParent(dieHolder.transform, true);
            die.GetComponent<DiePrefab>().dieData = abilityData.BASE_DICE[i];
        }
    }

    // UI elements should update last
    public int GetPriority(){return 1;}
    public void HandleEvent(CombatEventData eventData){}
}
