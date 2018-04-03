using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {


	public CombatController combatController;
	[HideInInspector]
	public Enemy targetedEnemy;


	public GameObject DefaultButtons, AbilityButtons, ItemMenu, textBox;
	public Button focusButton, overloadButton; //Drag focus and overload buttons to menuController
	public PlayerCombatScript player;
	public GameObject enemyPartCanvas;
	public GameObject [] enemyPartCanvasButtons;
	public bool focusEnabled, overloadEnabled;
	int enemyTargetNumber;
	public Image playerHealthFill, playerManaFill;
	public Text playerHealthText, playerManaText, textBoxText;
	public GameObject enemyHealthBarParent;
	public List<GameObject> enemyHealthBars;
	List<Image> enemyHealthFills;
	List<Text> enemyHealthTexts;
	public GameObject playerPovCamera;
	public float textSpeed;
	public bool proceed;
	public int selectedPart;
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

	//Doesn't actually do anything yet
	//Uses the linked list concept, to switch targets
	public void SelectTargetEnemy(int enemyNbr){
		targetedEnemy = combatController.enemyList[enemyNbr];
		foreach (var item in enemyHealthBars)
		{
			item.GetComponent<EnemyHealthBarScript>().targetbutton.interactable = true;
		}
		enemyHealthBars[combatController.enemyList.Count-1-enemyNbr].GetComponent<EnemyHealthBarScript>().targetbutton.interactable = false;
	}

 	//Buttons for button menus.

	public void AbilitiesMenu() {
		DefaultButtons.SetActive (false);
		AbilityButtons.SetActive (true);
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
	}
	public void PlayersTurn(){
		StartCoroutine(AttackWaitTime());
	}

	// Buttons.
	public void Attack () {
		DefaultButtons.SetActive(false);
		//player.Attack ();
		StartCoroutine(CameraToEnemy());
	}

	IEnumerator CameraToEnemy(){
		combatController.cameraScript.MoveCamera(targetedEnemy.cameraTarget);
		yield return new WaitUntil(()=>proceed);
		combatController.ActivatePartCanvas(targetedEnemy);
	}
	public void ChoosePartToAttack(){
		//player.Attack(part);
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
		playerHealthText.text = health.ToString() + "/" + maxHealth.ToString();
	}
	public void updateEnemyHealth(float health, float maxHealth, float percentage, Enemy enemyForListSearch){
		enemyHealthBars[enemyHealthBars.Count-1-(combatController.enemyList.IndexOf(enemyForListSearch))].GetComponent<EnemyHealthBarScript>().healthImage.fillAmount = percentage;
		enemyHealthBars[enemyHealthBars.Count-1-(combatController.enemyList.IndexOf(enemyForListSearch))].GetComponent<EnemyHealthBarScript>().healthText.text = health.ToString() +"/"+maxHealth.ToString();
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
	public void SelectEnemyPart(int partNbr){
		foreach (var item in enemyPartCanvasButtons)
		{
			item.GetComponent<Button>().interactable = true;
		}
		enemyPartCanvasButtons[partNbr].GetComponent<Button>().interactable = false;
		selectedPart = partNbr;
	}
	IEnumerator AttackWaitTime(){
		yield return new WaitForSeconds(1.5f);
		PlayerTurnTextFade();
		DefaultButtons.SetActive(true);
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

}
