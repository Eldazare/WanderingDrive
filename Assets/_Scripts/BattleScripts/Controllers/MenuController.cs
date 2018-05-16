using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour {

	//UI colors
	Color textColor = Color.white;

	public CombatController combatController; //Drag from Hierarchy
	[HideInInspector]
	public Enemy targetedEnemy;

	public GameObject DefaultButtons, AbilityButtons, ItemMenu, textBox, targetHealthBar; //Drag from Hierarchy
	public Button focusButton, overloadButton, abilityMenuButton, playerProfileButton; //Drag from Hierarchy
	public PlayerCombatScript player; //Drag from Hierarchy
	public GameObject enemyPartCanvas; //Drag from Hierarchy
	public List<GameObject> enemyPartCanvasButtons; //Drag from Hierarchy
	public List<Button> abilityButtons; //Drag from Hierarchy
	public List<Button> itemButtons; //Drag from Hierarchy
	public bool focusEnabled, overloadEnabled;
	int enemyTargetNumber;
	public Image playerHealthFill, playerHealthRedFill, playerStaminaFill; //Drag from Hierarchy
	public Text playerHealthText, playerStaminaText, textBoxText; //Drag from Hierarchy
	public GameObject enemyHealthBarParent; //Drag from Hierarchy
	public GameObject victoryScreen, loseScreen, runAwayScreen; //Drag from Hierarchy
	[HideInInspector]
	public List<GameObject> enemyHealthBars;
	List<Image> enemyHealthFills;
	List<Text> enemyHealthTexts;
	public GameObject playerPovCamera; //Drag from Hierarchy
	public float textSpeed = 0.02f;
	public bool proceed, abilityOrAttack;
	public int selectedPart = -1;
	public Text enemyTurnText, playerTurnText; //Drag from Hierarchy
	Color originalColor;

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
		foreach (var item in player.playerStats.abilities) {
			abilityButtons[player.playerStats.abilities.IndexOf (item)].interactable = true;
			abilityButtons[player.playerStats.abilities.IndexOf (item)].GetComponentInChildren<Text> ().text = item.abilityName;
		}
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
		DefaultButtons.SetActive (false);
		ItemMenu.SetActive (true);
		foreach (var item in player.playerStats.combatItems) {
			itemButtons[player.playerStats.combatItems.IndexOf (item)].interactable = true;
			itemButtons[player.playerStats.combatItems.IndexOf (item)].GetComponentInChildren<Text> ().text = item.GetType ().ToString ();
		}
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
		enemyPartCanvas.SetActive (false);
		if (player.paralyzed) {
			abilityMenuButton.enabled = false;
		} else {
			abilityMenuButton.enabled = true;
		}
	}
	public void PlayersTurn () {
		StartCoroutine (PlayerTurnRoutine ());
	}

	// Buttons.
	public void Attack () {
		DefaultButtons.SetActive (false);
		abilityOrAttack = false;
		//StartCoroutine (CameraToEnemy ());
		combatController.ActivatePartCanvas (targetedEnemy);
	}
	public void RunAwayButton () {
		combatController.RunAway ();
	}
	//Legacy routine for camera movement
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

	//Plays attack/ability animation and deactivates part canvas
	public void ChoosePartToAttack () {
		if (abilityOrAttack) {
			if (player.playerStats.abilities[player.abilityID].staminaCost < player.playerStats.stamina) {
				Debug.Log (selectedPart);
				player.Ability (selectedPart);
				combatController.ActivatePartCanvas (targetedEnemy);
			} else {
				WriteText ("Not enough stamina!");
			}

		} else {
			player.Attack (selectedPart);
			combatController.ActivatePartCanvas (targetedEnemy);
		}
		//StartCoroutine (PlayerAttack ());
	}

	//Legacy routine for camera movement
	/* IEnumerator PlayerAttack () {
		proceed = false;
		combatController.cameraScript.MoveCamera (playerPovCamera);
		yield return new WaitUntil (() => proceed);
		combatController.cameraScript.FollowTarget (playerPovCamera);

		if (abilityOrAttack) {
			player.Ability (selectedPart);
		} else {
			player.Attack (selectedPart);
		}
	} */

	public void Ability (int slot) {
		Ability ability = player.playerStats.abilities[slot];
		abilityOrAttack = true;
		player.abilityID = slot;
		AbilityButtons.SetActive (false);
		if (ability.offensive) {
			//player.CalculateDamage (AttackMode.Ability);
			//targetHealthBar.SetActive (true);
			targetHealthBar.GetComponent<TargetEnemyHealthBar> ().UpdateBar (selectedPart); 
			//StartCoroutine (CameraToEnemy ());
			combatController.ActivatePartCanvas (targetedEnemy);
		} else {
			player.Ability (0);
		}
	}

	public void Consumable (int slot) {
		ItemMenu.SetActive (false);
		player.CombatItem (slot);
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

	//Player's UpdateStats method calles this with player's health and max health
	public void UpdatePlayerHealth (float health, float maxHealth, float percentage) {
		playerHealthFill.fillAmount = percentage;
		playerHealthText.text = health.ToString ("0.#") + "/" + maxHealth.ToString ("0");
		playerStaminaText.text = player.playerStats.stamina.ToString ("0.#") + "/" + player.playerStats.maxStamina.ToString ("0");
		StartCoroutine (LerpStatusBar (playerHealthRedFill, playerHealthFill.fillAmount));
		StartCoroutine (LerpStatusBar (playerStaminaFill, player.playerStats.stamina / player.playerStats.maxStamina));
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

	//Player's turn routine
	IEnumerator PlayerTurnRoutine () {
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
	static public string DamageTextColor (string message) {
		string[] stringList = message.Split (' ');
		switch (stringList[3]) {
			case "Earth":
				message = stringList[0] + " " + stringList[1] + " " + "<color=brown>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			case "Fire":
				message = stringList[0] + " " + stringList[1] + " " + "<color=red>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			case "Ice":
				message = stringList[0] + " " + stringList[1] + " " + "<color=blue>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			case "Light":
				message = stringList[0] + " " + stringList[1] + " " + "<color=yellow>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			case "Shadow":
				message = stringList[0] + " " + stringList[1] + " " + "<color=purple>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			case "Thunder":
				message = stringList[0] + " " + stringList[1] + " " + "<color=darkblue>" + stringList[2] + " " + stringList[3] + "</color>";
				break;
			default:
				break;
		}
		return message;
	}
}