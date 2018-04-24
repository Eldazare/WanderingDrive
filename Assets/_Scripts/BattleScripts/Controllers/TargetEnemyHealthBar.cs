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
		foreach (var item in menu.player.attackList) {
			if (item != null) {
				damageTaken += menu.targetedEnemy.DamageTakenCalculation (item.damage, item.elementDamage, item.element, part);
			}
		}
		bar.healthImage.fillAmount = (menu.targetedEnemy.enemyStats.health - damageTaken) / menu.targetedEnemy.enemyStats.maxHealth;
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
		yield return new WaitForSeconds(1f);
		while(bar.healthFill2.fillAmount > bar.healthImage.fillAmount){
			bar.healthText.text = menu.targetedEnemy.enemyStats.health.ToString ("0.#") + " -> " + (menu.targetedEnemy.enemyStats.health-damageTaken).ToString ("0.#");
			bar.healthFill2.fillAmount -= Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}
}