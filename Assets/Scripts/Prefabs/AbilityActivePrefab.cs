using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActivePrefab : MonoBehaviour, IEventSubscriber
{
    // Backing variable required, see https://stackoverflow.com/a/3276167
    private AbstractAbility _abilityData;
    public AbstractAbility abilityData{
        get => _abilityData;
        set {_abilityData = value; this.InitialRender();}
    }

    void InitialRender(){

    }

    // UI elements should update last
    public int GetPriority(){return 1;}
    public void HandleEvent(CombatEventData eventData){}
}
