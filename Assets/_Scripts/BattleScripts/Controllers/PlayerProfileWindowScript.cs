using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileWindowScript : MonoBehaviour {

	public GameObject statusContent, playerProfileWindow; //Dragged from hierarchy
	public PlayerCombatScript player;	//Dragged from hierarchy
	public Dictionary<_Buff,GameObject> buffDescList = new Dictionary<_Buff, GameObject>();
	public void OpenPlayerProfile(){
		if(playerProfileWindow.activeInHierarchy){
			playerProfileWindow.SetActive(false);
		}else{
			playerProfileWindow.SetActive(true);

			//If buffDescList has a buff that is not in player buffs anymore, then the buff is removed from buffDescList aswell and it destroys the statusInfo Object
			foreach (KeyValuePair<_Buff, GameObject> item in buffDescList)
			{
				if(item.Key != null){
					if(!player.playerBuffs.Contains(item.Key)){
						Destroy(item.Value);
						buffDescList.Remove(item.Key);
					}
				}
			}
			foreach (var item in player.playerBuffs)
			{
				if (item != null){
					if(!buffDescList.ContainsKey(item)){
						GameObject status  = Instantiate(Resources.Load("CombatResources/Status"), statusContent.transform) as GameObject;
						buffDescList.Add(item, status);
						status.GetComponent<StatusEffectInfo>().statusName.text = item.GetType().Name;
						status.GetComponent<StatusEffectInfo>().statusDesc.text = "Potency: "+item.potency;
						//status.GetComponent<StatusEffectInfo>().statusDesc.text = //Datamanger.GetBuffDescription() + item.potency;
					}
				}
			}
		}
	}
}
