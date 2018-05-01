using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum AttackMode {
	Attack,
	Ability,
	Item
};
public class MenuController : MonoBehaviour {

	public CombatController combatController; //Drag from Hierarchy
	[HideInInspector]
	public Enemy targetedEnemy;

	public GameObject DefaultButtons, AbilityButtons, ItemMenu, textBox, targetHealthBar; //Drag from Hierarchy
	public Button focusButton, overloadButton, abilityMenuButton, playerProfileButton; //Drag buttons to menuController
	public PlayerCombatScript player; //Drag from Hierarchy
	public GameObject enemyPartCanvas; //Drag from Hierarchy
	public List<GameObject> enemyPartCanvasButtons; //Drag from Hierarchy
	public bool focusEnabled, overloadEnabled;
	int enemyTargetNumber;
	public Image playerHealthFill, playerHealthRedFill, playerManaFill; //Drag from Hierarchy
	public Text playerHealthText, playerManaText, textBoxText; //Drag from Hierarchy
	public GameObject enemyHealthBarParent; //Drag from Hierarchy
	public GameObject victoryScreen, loseScreen, runAwayScreen;
	[HideInInspector]
	public List<GameObject> enemyHealthBars;
	List<Image> enemyHealthFills;
	List<Text> enemyHealthTexts;
	public GameObject playerPovCamera; //Drag from Hierarchy
	public float textSpeed = 0.02f;
	public bool proceed, abilityOrAttack;
	public int selectedPart = -1, AbilityID;
	public Text enemyTurnText, playerTurnText;
	Color originalColor;
	public AttackMode attackMode;

	void Start () {
	focusEnabled = true;
	overloadEnabled = true;
	}
	public void GenerateHealthBars (int number, Enemy item) {
		GameObject newHealthBar = Instantiate (Resources.Load ("CombatResources/EnemyHealthBar"), transform.position, Quaternion.identity, enemyHealthBarParent.transform) as GameObject;
		enemyHealthBars.Add (newHealthBar);
		EnemyHealthBarScript newScript = newHealthBar.GetComponent<EnemyHealthBarScript> ();
		newScript.targetbutton.onClick.AddListener (delegate { SelectTargetEnemy (number); });
		newScript.buttonText.text = item.enemyName;
	}
	//Buttons for button menus.
	public void SelectTargetEnemy (int enemyNbr) {
		targetedEnemy = combatController.enemyList[enemyNbr];
		int i = enemyHealthBars.Count - 1;
		foreach (var item in combatController.enemyList) {
			if (item != null) {
				enemyHealthBars[i].SetActive (true);
				enemyHealthBars[i].GetComponent<EnemyHealthBarScript> ().targetbutton.interactable = true;
			} else {
				enemyHealthBars[i].SetActive (false);
			}
			i--;
		}
		player.transform.LookAt (targetedEnemy.transform.position);
		enemyHealthBars[combatController.enemyList.Count - 1 - enemyNbr].GetComponent<EnemyHealthBarScript> ().targetbutton.interactable = false;
	}
	public void AbilitiesMenu () {
		DefaultButtons.SetActive (false);
		AbilityButtons.SetActive (true);
		if (player.frozen) {
			overloadEnabled = false;
		}
		if (player.confused) {
			focusEnabled = false;
		}
		focusButton.interactable = focusEnabled;
		overloadButton.interactable = overloadEnabled;
	}
	public void ItemsMenu () {
		attackMode = AttackMode.Item;
		ItemMenu.SetActive (true);
		DefaultButtons.SetActive (false);
		//targetHealthBar.SetActive(true);
		//targetHealthBar.GetComponent<TargetEnemyHealthBar>().UpdateBar(selectedPart);
	}
	public void Back () {
		DefaultButtons.SetActive (true);
		targetHealthBar.SetActive (false);
		UpdateAllEnemyStats ();
		ItemMenu.SetActive (false);
		AbilityButtons.SetActive (false);
		//combatController.cameraScript.ResetCamera ();
		enemyPartCanvas.SetActive(false);
		if (player.paralyzed) {
			abilityMenuButton.enabled = false;
		} else {
			abilityMenuButton.enabled = true;
		}
	}
	public void PlayersTurn () {
		StartCoroutine (AttackWaitTime ());
	}

	// Buttons.
	public void Attack () {
		player.CalculateDamage (AttackMode.Attack);
		DefaultButtons.SetActive (false);
		abilityOrAttack = false;
		//StartCoroutine (CameraToEnemy ());
		combatController.ActivatePartCanvas (targetedEnemy);
	}
	public void RunAwayButton () {
		combatController.RunAway ();
	}

	/* IEnumerator CameraToEnemy () {
		combatController.cameraScript.MoveCamera (targetedEnemy.cameraTarget);
		yield return new WaitUntil (() => proceed);
		proceed = false;
		combatController.ActivatePartCanvas (targetedEnemy);
	} */

	public void SelectEnemyPart (int partNbr) {
		foreach (var item in enemyPartCanvasButtons) {
			item.GetComponent<Button> ().interactable = true;
		}
		enemyPartCanvasButtons[partNbr].GetComponent<Button> ().interactable = false;
		selectedPart = partNbr;
		targetHealthBar.GetComponent<TargetEnemyHealthBar> ().UpdateBar (partNbr);
	}
	public void ChoosePartToAttack () {
		combatController.ActivatePartCanvas (targetedEnemy);
		
		if (abilityOrAttack) {
			player.Ability (selectedPart);
		} else {
			player.Attack (selectedPart);
		}
		//StartCoroutine (PlayerAttack ());
	}
	IEnumerator PlayerAttack () {
		proceed = false;
		combatController.cameraScript.MoveCamera (playerPovCamera);
		yield return new WaitUntil (() => proceed);
		combatController.cameraScript.FollowTarget (playerPovCamera);

		if (abilityOrAttack) {
			player.Ability (selectedPart);
		} else {
			player.Attack (selectedPart);
		}
	}
	public void Ability (int slot) {
		player.CalculateDamage(AttackMode.Ability);
		AbilityButtons.SetActive (false);
		abilityOrAttack = true;
		player.abilityID = slot;/* 
		targetHealthBar.SetActive (true);
		targetHealthBar.GetComponent<TargetEnemyHealthBar> ().UpdateBar (selectedPart); */
		//StartCoroutine (CameraToEnemy ());
		combatController.ActivatePartCanvas (targetedEnemy);
	}

	public void Consumable (int slot) {
		ItemMenu.SetActive (false);
		player.Consumable (slot);
	}
	public void Focus () {
		AbilityButtons.SetActive (false);
		targetHealthBar.SetActive (false);
		player.PlayerFocus ();
	}
	public void Overload () {
		AbilityButtons.SetActive (false);
		targetHealthBar.SetActive (false);
		player.PlayerOverload ();
	}
	//UI updates
	public void UpdatePlayerHealth (float health, float maxHealth, float percentage) {
		playerHealthFill.fillAmount = percentage;
		playerHealthText.text = health.ToString ("0.#") + "/" + maxHealth.ToString ("0");
		StartCoroutine (LerpStatusBar (playerHealthRedFill, playerHealthFill.fillAmount));
	}
	public void UpdateEnemyHealth (float health, float maxHealth, float percentage, Enemy enemyForListSearch) {
		EnemyHealthBarScript healthbar = enemyHealthBars[enemyHealthBars.Count - 1 - (combatController.enemyList.IndexOf (enemyForListSearch))].GetComponent<EnemyHealthBarScript> ();
		healthbar.healthImage.fillAmount = percentage;
		healthbar.healthText.text = health.ToString ("0.#") + "/" + maxHealth.ToString ("0");
		StartCoroutine (LerpStatusBar (healthbar.healthFill2, healthbar.healthImage.fillAmount));
	}
	IEnumerator LerpStatusBar (Image fill, float goal) {
		yield return new WaitForSeconds (1f);
		while (fill.fillAmount > goal) {
			fill.fillAmount -= Time.deltaTime;
			yield return new WaitForSeconds (Time.deltaTime);
		}
	}

	public void MessageToScreen (string message) {
		StartCoroutine (WriteText (message));
	}
	IEnumerator WriteText (string message) {
		textBox.SetActive (true);
		textBoxText.text = "";
		/* foreach (var item in message)
		{
			textBoxText.text += item;
			yield return new WaitForSeconds(textSpeed);
		} */
		textBoxText.text = message;
		yield return new WaitForSeconds (1.5f);
		textBoxText.text = "";
		textBox.SetActive (false);
	}

	IEnumerator AttackWaitTime () {
		proceed = false;
		player.ApplyPlayerBuffs ();
		yield return new WaitUntil (() => proceed);
		if (!player.stunned) {
			PlayerTurnTextFade ();
			DefaultButtons.SetActive (true);
			UpdateAllEnemyStats ();
			if (player.paralyzed) {
				abilityMenuButton.enabled = false;
			} else {
				abilityMenuButton.enabled = true;
			}
			//Enable enemy healthbars
			int i = enemyHealthBars.Count - 1;
			foreach (var item in combatController.enemyList) {
				if (item != null) {
					enemyHealthBars[i].SetActive (true);
					if (item == targetedEnemy) {
						enemyHealthBars[i].GetComponent<EnemyHealthBarScript> ().targetbutton.interactable = false;
					} else {
						enemyHealthBars[i].GetComponent<EnemyHealthBarScript> ().targetbutton.interactable = true;
					}
				} else {
					enemyHealthBars[i].SetActive (false);
					if (targetedEnemy == null) {
						foreach (var item1 in combatController.enemyList) {
							if (item != null) {
								targetedEnemy = item1;
								break;
							}
						}
					}
				}
				i--;
			}
		} else {
			GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (player.transform.position.x, player.transform.position.y + 3, player.transform.position.z) - player.transform.right, Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh> ().text = "Stunned";
			yield return new WaitForSeconds (1f);
			combatController.EnemyAttacks ();
		}
	}
	void UpdateAllEnemyStats () {
		foreach (var enemy in combatController.enemyList) {
			if (enemy != null) {
				enemy.updateStats ();
			}
		}
	}
	public void EnemyTurnTextFade () {
		enemyTurnText.gameObject.SetActive (true);
		enemyTurnText.CrossFadeAlpha (1.0f, 0.0f, false);
		enemyTurnText.CrossFadeAlpha (0.0f, 3.0f, false);
	}

	void PlayerTurnTextFade () {
		playerTurnText.gameObject.SetActive (true);
		playerTurnText.CrossFadeAlpha (1.0f, 0.0f, false);
		playerTurnText.CrossFadeAlpha (0.0f, 3.0f, false);
	}
}