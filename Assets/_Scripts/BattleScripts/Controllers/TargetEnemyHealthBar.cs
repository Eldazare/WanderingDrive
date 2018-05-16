using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemyHealthBar : MonoBehaviour {

	public EnemyHealthBarScript hpbar;
	public MenuController menu;
	float damageTaken;
	void OnEnable () {
		UpdateBar (0);
	}
	public void UpdateBar (int part) {
		EnemyHealthBarScript bar = GetComponent<EnemyHealthBarScript> ();
		damageTaken = 0;
		foreach (var attack in menu.player.attackList) {
			if (attack != null) {
				AttackResult result = menu.combatController.UniversalDamageTaken(true, attack) ;
				damageTaken += result.damage+result.elementDamage;
			}
		}
		bar.healthImage.fillAmount = (menu.targetedEnemy.enemyStats.health - (damageTaken)) / menu.targetedEnemy.enemyStats.maxHealth;
		bar.healthText.text = menu.targetedEnemy.enemyStats.health.ToString ("0.#") + " -> " + (menu.targetedEnemy.enemyStats.health-damageTaken).ToString ("0.#");
		bar.healthFill2.fillAmount = menu.targetedEnemy.enemyStats.health / menu.targetedEnemy.enemyStats.maxHealth;
		bar.buttonText.text = menu.targetedEnemy.enemyName;
	}
	public void UpdateCurrentHP(){
		StartCoroutine(LerpStatusBar());
	}
	IEnumerator LerpStatusBar(){
		EnemyHealthBarScript bar = GetComponent<EnemyHealthBarScript> ();
		bar.healthImage.fillAmount = menu.targetedEnemy.enemyStats.health / menu.targetedEnemy.enemyStats.maxHealth;
		while(bar.healthFill2.fillAmount > bar.healthImage.fillAmount){
			bar.healthFill2.fillAmount -= Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime/2);
		}
		yield return new WaitForSeconds(0.5f);
		gameObject.SetActive(false);
	}
}