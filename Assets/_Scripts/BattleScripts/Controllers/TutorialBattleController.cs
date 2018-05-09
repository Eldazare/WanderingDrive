using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialBattleController : MonoBehaviour {

	public GameObject textBox;
	public Text textBoxText;
	float textSpeed = 0.02f;
	public GameObject tapToContinue;
	public GameObject blockerAll, blockerLeft, blockerRight, blockerButtons, blockerStatus, blockerFocus, blockerNoClick;
	bool waitingForClickTextBox, proceed, proceedTutorial;
	public MenuController menu; //Drag from Hierarchy
	public PlayerProfileWindowScript playerProfile;
	bool skipped5, skipped12, skipped15;
	public int tutorialStep;
	static string tutorialText1 = "Welcome to Hunting monsters!\nLet's go through the mechanics of the fight!";
	static string tutorialText2 = "Combat is Turn-Based so you can take your time thinking about your turn.";
	static string tutorialText3 = "But when enemy attacks you have to time your defensive action with enemy's attack!\nLearn the timings on different enemy attacks to get a bigger advantage.";
	static string tutorialText4 = "Defensive actions you can take are Block and Dodge. You can only perform one of them once for each enemy attack, so don't tap away carelessly!";
	static string tutorialText5 = "Block\nTap your screen just before enemy hits you to block the attack. Tapping starts a 2-second timer so if you are not sure about the enemy, just tap on time before the attack.";
	static string tutorialText55 = "Block has 4 tiers of effectiveness: 100%, 75%, 50% and 25% damage reduction depending how well you time your defence.";
	static string tutorialText6 = "Dodge\nSwipe your screen just before enemy hits you to dodge the attack. Dodge is all or nothing defensive action. You either succeed on dodging the attack entirely or you take full damage.";
	static string tutorialText7 = "For offensive action you can choose from attacking, using ability or item . Normal attack starts a combo that you have to tap and swipe correctly in order to deal maximum amount of damage.";
	static string tutorialText8 = "You have two core abilities: Focus and Overload.";
	static string tutorialText9 = "Focus\nYou give up your turn but you gain ease to your defensive actions and you gain damage bonus of 50% and an extra turn after your next one.";
	static string tutorialText10 = "Overload\nYou gain 2 turns immediately and gain damage bonus of 50% for them but you will forfeit your next turn and you'll be unable to dodge for the enemy turns";
	static string tutorialText11 = "You can use Focus and Overload together for a unique interaction after figuring it out.";
	static string tutorialText12 = "Okay so here on the left you have your status bars and your character. You can click on your portrait to see any status effects affecting you as well as some more information about your abilities if you forget.";
	static string tutorialText122 = "";
	static string tutorialText13 = "On the right you have your enemies. If you are facing multiple monsters you can select which one you want to attack by clicking the name below their health bar.";
	static string tutorialText14 = "Enemies have different parts that have their own health, attack timings and more that you should try to learn to improve your hunting skills.";
	static string tutorialText15 = "After selecting an enemy and selecting your course of action. You can select the enemy's part that you want to target.";
	static string tutorialText155 = "Enemy's parts have their own health points and breaking a part will gain you that part regardless of if you win, lose or run away." +
		" Different parts have the chance to hit on them and also take different damages. More risk, more reward.";
	static string tutorialText16 = "Now try to win the battle. Attack the enemy, complete combo by tapping and swiping to correct direction. After attacking be ready to swipe or tap to defend against enemy attack. Good luck!";
	List<string> tutorialTexts = new List<string> () { tutorialText1, tutorialText2, tutorialText3, tutorialText4, tutorialText5, tutorialText55, tutorialText6, tutorialText7, tutorialText8,
	 tutorialText9, tutorialText10, tutorialText11, tutorialText12, tutorialText122, tutorialText13, tutorialText14, tutorialText15, tutorialText155, tutorialText16 };
	// Use this for initialization
	void Start () {
		Debug.Log("start tutorial");
		StartTutorial();
	}

	public void StartTutorial(){
		StartCoroutine(StartBattleTutorial());
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			if (waitingForClickTextBox) {
				proceed = true;
			}
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (waitingForClickTextBox) {
				proceed = true;
			}
		}
	}
	IEnumerator StartBattleTutorial () {
		tutorialStep = 1;
		foreach (var item in tutorialTexts) {
			StartCoroutine(WriteText (item));
			switch (tutorialStep) {
				case 1:
					blockerNoClick.SetActive (true);
					blockerAll.SetActive (true);
					break;
				case 5:
					if(!skipped5){
						tutorialStep--;
						skipped5 = true;
					}
					break;
				case 7:
					menu.Back();
					blockerAll.SetActive (false);
					blockerButtons.SetActive (true);
					break;
				case 8:
					menu.AbilitiesMenu();
					blockerButtons.SetActive (false);
					blockerFocus.SetActive (true);
					break;
				case 12:
					if(skipped12){
						StopCoroutine("WriteText");
						textBox.SetActive (false);
						playerProfile.OpenPlayerProfile();
					}else{
						menu.Back ();
						blockerFocus.SetActive (false);
						blockerRight.SetActive(true);
						skipped12 = true;
						tutorialStep--;
					}
					break;
				case 13:
					playerProfile.OpenPlayerProfile();
					blockerRight.SetActive(false);
					blockerLeft.SetActive(true);
					break;
				case 14:
					
					
					break;
				case 15:
					menu.Attack();
					break;
				case 16:
					if(!skipped15){
						tutorialStep--;
						skipped15 = true;
					}
					break;
				case 17:
					menu.Back();
					blockerLeft.SetActive(false);
					break;
				default:
					break;
			}
			yield return new WaitUntil (() => proceedTutorial);
			proceedTutorial = false;
			tutorialStep++;
		}
		blockerLeft.SetActive(false);
		textBox.SetActive(false);
		blockerNoClick.SetActive(false);
	}

	IEnumerator WriteText (string message) {
		textBox.SetActive (true);
		textBoxText.text = "";
		waitingForClickTextBox = true;
		foreach (var item in message) {
			if (proceed) {
				break;
			}
			textBoxText.text += item;
			yield return new WaitForSeconds (textSpeed);
		}
		proceed = false;
		waitingForClickTextBox = false;
		textBoxText.text = message;
		yield return new WaitForSeconds (0.5f);
		waitingForClickTextBox = true;
		tapToContinue.SetActive (true);
		yield return new WaitUntil (() => proceed);
		tapToContinue.SetActive (false);
		proceed = false;
		waitingForClickTextBox = false;
		textBoxText.text = "";
		proceedTutorial = true;
	}
	/* IEnumerator WaitForProceeding(float wait){
		yield return new WaitForSeconds(wait);
		if(!proceed){	
			proceed = true;
		}
		yield return new WaitForSeconds(Time.deltaTime);
		proceed = false;
	} */
}