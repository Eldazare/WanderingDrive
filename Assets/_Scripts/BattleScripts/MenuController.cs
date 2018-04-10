using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {


	public CombatController combatController;  //Drag from Hierarchy
	[HideInInspector]
	public Enemy targetedEnemy;


	public GameObject DefaultButtons, AbilityButtons, ItemMenu, textBox;  //Drag from Hierarchy
	public Button focusButton, overloadButton, abilityMenuButton; //Drag buttons to menuController
	public PlayerCombatScript player;  //Drag from Hierarchy
	public GameObject enemyPartCanvas;  //Drag from Hierarchy
	public List<GameObject> enemyPartCanvasButtons;  //Drag from Hierarchy
	public bool focusEnabled, overloadEnabled;
	int enemyTargetNumber;
	public Image playerHealthFill, playerManaFill;  //Drag from Hierarchy
	public Text playerHealthText, playerManaText, textBoxText;  //Drag from Hierarchy
	public GameObject enemyHealthBarParent;  //Drag from Hierarchy
	[HideInInspector]
	public List<GameObject> enemyHealthBars;
	List<Image> enemyHealthFills;
	List<Text> enemyHealthTexts;
	public GameObject playerPovCamera; //Drag from Hierarchy
	public float textSpeed = 0.02f;
	public bool proceed;
	public int selectedPart = -1;
	public Text enemyTurnText, playerTurnText;
	Color originalColor;
	void Start(){
		focusEnabled = true;
		overloadEnabled = true;
	}	

	public void GenerateHealthBars(int number, Enemy item){
		GameObject newHealthBar = Instantiate(Resources.Load("CombatResources/EnemyHealthBar"),transform.position, Quaternion.identity,enemyHealthBarParent.transform) as GameObject;
		enemyHealthBars.Add(newHealthBar);
		EnemyHealthBarScript newScript = newHealthBar.GetComponent<EnemyHealthBarScript>();
		newScript.targetbutton.onClick.AddListener(delegate{SelectTargetEnemy(number);});
		newScript.buttonText.text = item.enemyName;
	}
		//Buttons for button menus.
	public void SelectTargetEnemy(int enemyNbr){
		targetedEnemy = combatController.enemyList[enemyNbr];
		int i = enemyHealthBars.Count-1;
		foreach (var item in combatController.enemyList){
			if(item != null){
				enemyHealthBars[i].SetActive(true);
				enemyHealthBars[i].GetComponent<EnemyHealthBarScript>().targetbutton.interactable = true;
			}else{
				enemyHealthBars[i].SetActive(false);
			}
			i--;
		}
		enemyHealthBars[combatController.enemyList.Count-1-enemyNbr].GetComponent<EnemyHealthBarScript>().targetbutton.interactable = false;
	}
	public void AbilitiesMenu() {
		DefaultButtons.SetActive (false);
		AbilityButtons.SetActive (true);
		if(player.frozen){
			overloadEnabled = false;
		}
		if(player.confused){
			focusEnabled = false;
		}
		focusButton.interactable = focusEnabled;
		overloadButton.interactable = overloadEnabled;
	}
	public void ItemsMenu () {
		ItemMenu.SetActive (true);
		DefaultButtons.SetActive (false);
	}
	public void Back () {
		DefaultButtons.SetActive (true);
		ItemMenu.SetActive (false);
		AbilityButtons.SetActive (false);
		combatController.cameraScript.ResetCamera();
		if(player.paralyzed){
			abilityMenuButton.enabled = false;
		}else{
			abilityMenuButton.enabled = true;
		}
	}
	public void PlayersTurn(){
		StartCoroutine(AttackWaitTime());
	}

	// Buttons.
	public void Attack () {
		DefaultButtons.SetActive(false);
		StartCoroutine(CameraToEnemy());
	}

	IEnumerator CameraToEnemy(){
		combatController.cameraScript.MoveCamera(targetedEnemy.cameraTarget);
		yield return new WaitUntil(()=>proceed);
		proceed = false;
		combatController.ActivatePartCanvas(targetedEnemy);
	}

	public void SelectEnemyPart(int partNbr){
		foreach (var item in enemyPartCanvasButtons)
		{
			item.GetComponent<Button>().interactable = true;
		}
		enemyPartCanvasButtons[partNbr].GetComponent<Button>().interactable = false;
		selectedPart = partNbr;
	}
	public void ChoosePartToAttack(){
		combatController.ActivatePartCanvas(targetedEnemy);
		StartCoroutine(PlayerAttack());
	}
	IEnumerator PlayerAttack(){
		proceed = false;
		combatController.cameraScript.MoveCamera(playerPovCamera);
		yield return new WaitUntil(()=>proceed);
		combatController.cameraScript.FollowTarget(playerPovCamera);
		player.Attack(selectedPart);
	}
	public void Ability(int slot) {
		AbilityButtons.SetActive (false);
		player.Ability (slot, selectedPart);
	}

	public void Consumable(int slot){
		ItemMenu.SetActive (false);
		player.Consumable(slot);
	}
	public void Focus() {
		AbilityButtons.SetActive (false);
		player.PlayerFocus ();
	}
	public void Overload () {
		AbilityButtons.SetActive (false);
		player.PlayerOverload ();
	}

	//UI updates
	public void updatePlayerHealth(float health, float maxHealth, float percentage){
		playerHealthFill.fillAmount = percentage;
		playerHealthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
	}
	public void updateEnemyHealth(float health, float maxHealth, float percentage, Enemy enemyForListSearch){
		enemyHealthBars[enemyHealthBars.Count-1-(combatController.enemyList.IndexOf(enemyForListSearch))].GetComponent<EnemyHealthBarScript>().healthImage.fillAmount = percentage;
		enemyHealthBars[enemyHealthBars.Count-1-(combatController.enemyList.IndexOf(enemyForListSearch))].GetComponent<EnemyHealthBarScript>().healthText.text = health.ToString("0") +"/"+maxHealth.ToString("00");
	}

	public void messageToScreen(string message){
		textBox.SetActive(true);
		StartCoroutine(WriteText(message));
	}
	IEnumerator WriteText(string message){
		foreach (var item in message)
		{
			textBoxText.text += item;
			yield return new WaitForSeconds(textSpeed);
		}
		yield return new WaitForSeconds(1.5f);
		textBoxText.text = "";
		textBox.SetActive(false);
	}
	
	IEnumerator AttackWaitTime(){
		proceed = false;
		player.ApplyPlayerBuffs();
		yield return new WaitUntil(()=>proceed);
		yield return new WaitForSeconds(1f);
		if(!player.stunned){
			PlayerTurnTextFade();
			DefaultButtons.SetActive(true);
			if(player.paralyzed){
				abilityMenuButton.enabled = false;
			}else{
				abilityMenuButton.enabled = true;
			}
			//Enable enemy healthbars
			int i = enemyHealthBars.Count-1;
			foreach (var item in combatController.enemyList){
				if(item != null){
					enemyHealthBars[i].SetActive(true);
					if(item == targetedEnemy){
						enemyHealthBars[i].GetComponent<EnemyHealthBarScript>().targetbutton.interactable = false;
					}else{
						enemyHealthBars[i].GetComponent<EnemyHealthBarScript>().targetbutton.interactable = true;
					}
				}else{
					enemyHealthBars[i].SetActive(false);
					if(targetedEnemy == null){
						foreach (var item1 in combatController.enemyList)
						{
							if(item != null){
								targetedEnemy = item1;
								break;
							}
						}
					}
				}
				i--;
			}
			
		}else{
			GameObject popup = Instantiate(Resources.Load("CombatResources/DamagePopUp"),new Vector3(player.transform.position.x, player.transform.position.y+3, player.transform.position.z)-player.transform.right, Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh>().text = "Stunned";
			yield return new WaitForSeconds(1f);
			combatController.enemyAttacks();
		}
	}

	public void EnemyTurnTextFade(){
		enemyTurnText.gameObject.SetActive(true);
		enemyTurnText.CrossFadeAlpha(1.0f, 0.0f, false);
		enemyTurnText.CrossFadeAlpha(0.0f, 3.0f,false);
	}

	void PlayerTurnTextFade(){
		playerTurnText.gameObject.SetActive(true);
		playerTurnText.CrossFadeAlpha(1.0f, 0.0f, false);
		playerTurnText.CrossFadeAlpha(0.0f, 3.0f, false);
	}

	public void RunAway(){
		//SceneManager.LoadScene("MapScene");
	}

}
