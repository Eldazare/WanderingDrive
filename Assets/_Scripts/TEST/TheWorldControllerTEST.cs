using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWorldControllerTEST : MonoBehaviour {

	public GameObject loadoutChoicePanel; // parent, true/false
	public GameObject loadoutsPanel; // sub, contains loadout buttons

	public GameObject ResourceDropPanel;
	public Text resourceDropText;

	public GameObject tutorialMasterPanel;
	public Text tutorialWorldText;

	int currentTutorialString;
	string[] tutorialTexts;

	public void DoBattle(){
		GameObject.FindGameObjectWithTag("TestNode").GetComponent<WorldNode> ().Interact ();
	}

	public void DoCraft(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().StartCrafting ();
	}

	public void DoUpgrade(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().StartUpgrading ();
	}

    public void DoLoadoutManagement() {
        GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().StartLoadoutManagement();
    }

	public void DoTutorialBattle(){
		GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().StartTutorial();
	}

    public void GenerateLoadoutButtons(){
		loadoutChoicePanel.SetActive (true);
		if (loadoutsPanel.transform.childCount == 0) {
			UndyingObject undObject = GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ();
			LoadoutsContainer container = undObject.loadoutList;
			GameObject buttonPrefab = Resources.Load ("CraftingUi/Button") as GameObject;
			for (int i = 0; i < container.GetLoadoutCount (); i++) {
				if (container.GetLoadout (i) != null) {
					GameObject button = Instantiate (buttonPrefab, loadoutsPanel.transform);
					Button but = button.GetComponent<Button> ();
					int ind = i;
					but.onClick.AddListener (delegate {
						undObject.ReceiveChosenLoadout (ind);
					});
					button.GetComponentInChildren<Text> ().text = "Loadout " + (i + 1);
				}
			}
		}
	}

	public void ReturnFromLoadoutMenu(){
		loadoutChoicePanel.SetActive (false);
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().NullNodeData ();
	}

	public void DoTutorial(){
		tutorialMasterPanel.SetActive (true);
		tutorialTexts = (Resources.Load ("DataText/WORLDTUTORIALTEXTS") as TextAsset).text.Split ("\n".ToCharArray());
		currentTutorialString = 0;
		SetTutorialText ();
	}

	public void SkipTutorial(){
		tutorialMasterPanel.SetActive (false);
	}

	public void NextTutorial(){
		currentTutorialString++;
		if (currentTutorialString >= tutorialTexts.Length) {
			currentTutorialString = tutorialTexts.Length - 1;
		}
		SetTutorialText ();
	}

	public void PrevTutorial(){
		currentTutorialString--;
		if (currentTutorialString < 0) {
			currentTutorialString = 0;
		}
		SetTutorialText ();
	}

	private void SetTutorialText(){
		tutorialWorldText.text = "Page: "+(currentTutorialString+1)+"/"+tutorialTexts.Length+"\n"+tutorialTexts [currentTutorialString];
	}

	public void SetResources(List<RecipeMaterial> materials){
		ResourceDropPanel.SetActive (true);
		string infoStuff = InfoBoxCreator.GetMatListInfo (materials);
		if (infoStuff != "") {
			resourceDropText.text = "Materials gained:\n" + infoStuff;
		} else {
			resourceDropText.text = "No materials gained.";
		}
	}

}
