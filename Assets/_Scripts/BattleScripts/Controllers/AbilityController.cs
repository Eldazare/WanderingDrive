using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour {

	public void AbilityEffect(string effectName){
        PlayerCombatScript player = GetComponent<PlayerCombatScript>();
        GameObject effect = Instantiate(Resources.Load("CombatResources/SpellEffects/"+effectName),player.transform.position, Quaternion.identity) as GameObject;
        effect.GetComponent<EffectType>().player = player;
        effect.GetComponent<EffectType>().StartEffect();
    }
}
