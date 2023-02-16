using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replicator : MonoBehaviour
{
    public List<string> abilitiesToApply = new List<string>();

    public GameObject nextPlayer;
    public GameObject nextPlayerPrefab;
    public GameObject gameController;

    public Camera camera;

    
    void Update(){
        /*
        if(nextPlayer == null){
            nextPlayer = (GameObject)Instantiate(nextPlayerPrefab, transform.position, Quaternion.identity);
            nextPlayer.name = "Player";
            nextPlayer.SetActive(false);
        }
        */

        if(Input.GetKeyDown("l")){
            CreateNewPlayer();
        }
    }
    

    public void CreateNewPlayer(){
            nextPlayer = (GameObject)Instantiate(nextPlayerPrefab, transform.position, Quaternion.identity);
            nextPlayer.name = "Player";
            List<GameObject> inReplicatorInventory = gameController.GetComponent<Inventory>().replicatorSlots;

            for(int i = 0; i < inReplicatorInventory.Count;i++){
                if(inReplicatorInventory[i].GetComponent<InventorySlot>().abilityObj == null){
                    break;
                }
                abilitiesToApply.Add(inReplicatorInventory[i].GetComponent<InventorySlot>().GetAbility());
            }

            ApplyAllAbilities();

            camera.GetComponent<ScreenCamera>().target = nextPlayer.transform;
            //nextPlayer.SetActive(false);
    }

    public void ApplyAllAbilities(){
        PlayerLogic playerLogic = nextPlayer.GetComponent<PlayerLogic>();

        for (int i = 0; i < abilitiesToApply.Count; i++){
            switch(abilitiesToApply[i]){
                case "speed" : playerLogic.RunSpeed += 50f;
                break;
                default : Debug.Log("Nem volt benne semmi");
                break;
            }
        }
    }



}
