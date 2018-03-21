using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour{
	GameObject weapon;
	PlayerStats playerStats;
	float blockTimer, blockDuration, dodgeTimer, dodgeDuration, timerAccuracy;


	void Start () {

	}

	public string GetHit(int damageTaken, bool area){
		if(dodgeTimer>0){
			if(area){
				playerStats.health -= damageTaken*playerStats.damageReduction;

				return "took"+damageTaken*playerStats.damageReduction+"damage";
			}
			else{
				return "dodged the attack!";
			}
		}
		else if(blockTimer>0){

			//Block laskut
			return "you blocked the attack but took damage!";
		}
		else{
			//Tehdään damage reduction lasku armorin ja element resistin mukaan
			playerStats.health -= damageTaken*playerStats.damageReduction;
			return "took"+damageTaken*playerStats.damageReduction+"damage";
		}
	}

	void BlockCountDown(){
		blockTimer -= timerAccuracy;
		if(blockTimer <= 0){
			blockTimer = 0;
			CancelInvoke("BlockCountDown");
		}
	}

	void DodgeCountDown(){
		dodgeTimer -= timerAccuracy;
		if(dodgeTimer <= 0){
			dodgeTimer = 0;
			CancelInvoke("DodgeCountDown");
		}
	}

	public void Dodge(){
		if(dodgeTimer<=0 && !IsInvoking("DodgeCountDown")){
			//Tee dodge
			dodgeTimer += dodgeDuration;
			InvokeRepeating("DodgeCountDown",0, timerAccuracy);
		}
	}

	public void Block(){
		if(blockTimer<=0 && !IsInvoking("BlockCountDown")){
			//Tee block
			blockTimer += blockDuration;
			InvokeRepeating("BlockCountDown", 0, timerAccuracy);
		}
	}
}
