using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreasures : MonoBehaviour  {

    public GameObject mineGrid;

    public void ShowTreasures() {
        
    }

    public void ReturnFromGathering() {
        //palauta
        List<RecipeMaterial> treasures = mineGrid.GetComponent<PopulateGrid>().items;
    }


}
