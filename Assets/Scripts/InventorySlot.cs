using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject abilityObj;
    public GameObject gameController;
    public Image image;
    private bool isPlayerInventory;

    void Awake(){
        gameController = GameObject.Find("Game Controller");
        if(this.transform.parent.name == "Inventory"){
            isPlayerInventory = true;
        }
        else{
            isPlayerInventory = false;
        }
    }
    public void UpdateImage(){
        image.gameObject.SetActive(true);
        image.sprite = abilityObj.GetComponent<SpriteRenderer>().sprite;
    }

    public void TransferItem(){

        if(abilityObj == null){
            return;
        }

        if(isPlayerInventory){
            gameController.GetComponent<Inventory>().AddToReplicator(abilityObj);
        }
        else{
            gameController.GetComponent<Inventory>().AddToInvertory(abilityObj);
        }

        this.abilityObj = null;
        image.sprite = null;
        image.gameObject.SetActive(false);
    }

    public string GetAbility(){
        return (abilityObj.GetComponent<Ability>().identifier);
    }

}
