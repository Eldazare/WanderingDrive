using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameController : MonoBehaviour {

	public int ForcedEncounter = -1;
	public List<GameObject> EnemyList;
	int ind;
	public GameObject Enemy;


	Enemy1 hostile;
	public GameObject Player;
	PlayerScript1 pleb;

	public Button Attack;
	public Button Defend;
	public Button Run;
	public Button Abilities;
	public Button Back;

	//public List<Button> ButtonList;

	public GameObject ButtonParent1;
	public GameObject ButtonParent2;


	public Text TextBox;
	public Text PlayerHP;
	public Text EnemyHP;


	bool PlayerTurn = true;
	bool Defended = false;

	public GameObject bar;
	public GameObject PlayerHPBar;
	Image HPfill;
	Image barFill;
	float modLength;


	public GameObject DefeatScreen, VictoryScreen, CowardScreen;
	public Text LootText;


	void Awake (){

		barFill = bar.GetComponent<Image> ();
		barFill.fillAmount = 0;
		HPfill = PlayerHPBar.GetComponent<Image>();

		// Picks a random enemy prefab from a list, instantiates it and sets it as a child of an empty object in order to get it in the right position.
		// We can also set a specific encounter.
		if (ForcedEncounter < 0) {
			ind = Random.Range (0, EnemyList.Count);
		} else {
			ind = ForcedEncounter;
		}
		Debug.Log ("Enemy:" + ind);
		GameObject e = Instantiate (EnemyList[ind], transform.position, Quaternion.identity);
		e.transform.SetParent (Enemy.transform);


		// Finds a player
		Player = GameObject.FindGameObjectWithTag ("Player");
	}



	// Use this for initialization
	void Start () {

		// Sets up buttons
		Attack.onClick.AddListener (() => Select (Attack));
		Defend.onClick.AddListener (() => Select (Defend));
		Run.onClick.AddListener (() => Select (Run));
		//Abilities.onClick.AddListener (() => Select (Abilities));
		//Back.onClick.AddListener (() => Select (Back));



		hostile = EnemyList[ind].GetComponent<Enemy1>();
		pleb = Player.GetComponent<PlayerScript1>();

		UpdateHealth ();
		//TextBox.text = "";


		string m = "Encountered " + hostile.Name;
		StartCoroutine (TextLine(m, TextBox));
	}
	


	void Update () {
		
	}




	// RepeatInvoke this
	void BarSet () {
		if (barFill.fillAmount > 0) {
			barFill.fillAmount -= 0.02f;
		} else {
			barFill.fillAmount = 0;
			CancelInvoke ("BarSet");
		}
	}


	// Updates health bars.
	void UpdateHealth(){
		PlayerHP.text = "HP: " + pleb.Health;
		EnemyHP.text = "HP: " + hostile.Health;

		Debug.Log ((float)pleb.Health / 100.0f);
		HPfill.fillAmount = (float)pleb.Health / 100.0f;



		if (pleb.Health <= 0) {
			//SceneManager.LoadScene ("Defeat");
			DefeatScreen.SetActive(true);
			GetComponent<AudioSource> ().Stop ();
		}
		if (hostile.Health <= 0) {
			//SceneManager.LoadScene ("Victory");
			VictoryScreen.SetActive(true);
			GetComponent<AudioSource> ().Stop ();


			string m = "You've found a... stick on a stick!";
			StartCoroutine (TextLine(m, LootText));

		}
	}

	public void Derp () {
		Debug.Log ("Derp");
	}

	// Button selecion check
	void Select (Button b) {
		if (PlayerTurn) {
			if (b == Attack) {
				string m = "Attacked for " + pleb.Attack + " damage.";
				StartCoroutine (TextLine (m, TextBox));

				hostile.Health -= pleb.Attack;
				PlayerTurn = false;
				StartCoroutine (AtkRoutine ());
				UpdateHealth ();

			}
			if (b == Run) {
				string m = "You ran away like a coward.";
				StartCoroutine (TextLine (m, TextBox));
				//SceneManager.LoadScene ("Ran");
				GetComponent<AudioSource> ().Stop ();
				CowardScreen.SetActive(true);
			}
			/*
			if (b == Abilities) {
				ButtonParent1.transform.position = new Vector3 (0, -300.0f, 0);
				ButtonParent2.transform.position = new Vector3 (0, 0, 0);
			}
			if (b == Back) {
				ButtonParent2.transform.position = new Vector3 (0, -300.0f, 0);
				ButtonParent1.transform.position = new Vector3 (0, 0, 0);
			}
			*/
		} else {
			if (b == Defend && bar.transform.localScale.x > 0) {
				Defended = true;
				modLength = barFill.fillAmount;
				Debug.Log ("Defend: " + modLength);
			}
		}
	}

	// Slow textlines
	IEnumerator TextLine(string mes, Text box){
		box.text = "";
		for (int i = 0; i < mes.Length; i++) {
			box.text += mes [i];
			yield return new WaitForSeconds (0.005f);
		}
	}

	// Enemy attacks with a delay
	IEnumerator AtkRoutine () {
		Defended = false;
		// sets the block timing bar
		barFill.fillAmount = 1.0f;
		InvokeRepeating("BarSet", 0.1f, 0.015f);
		yield return new WaitForSeconds (2.5f);
		string m;
		if (Defended == false) {
			// Player fails to block
			m = hostile.Name + " attacks for " + hostile.Attack + " damage.";
			pleb.Health -= hostile.Attack;
		} else {
			// Player blocks
			float mod = (float)hostile.Attack * (modLength);
			m = "You blocked the attack. You take " + (int)mod + " damage.";

			pleb.Health -= (int)mod;
		}

		StartCoroutine (TextLine(m, TextBox));
		UpdateHealth ();
		PlayerTurn = true;
	}
}
