using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject abilityObj;
    public GameObject gameController;
    public Image image;
    private bool isPlayerInventory;
    public GameObject tooltipUI;

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
        Debug.Log("transfering");
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(abilityObj != null){
            tooltipUI.SetActive(true);
            tooltipUI.GetComponent<DynamicUI>().SetText(abilityObj.GetComponent<Ability>().text);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipUI.SetActive(false);
    }

}
