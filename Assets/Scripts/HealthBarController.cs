using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private List<Heart> hearts = new List<Heart>();
    [SerializeField] public Image gameOverScreen;
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private Heart leftHeart;
    [SerializeField] private Heart rightHeart;
    [SerializeField] private GameController gameController;
    private float tmpHealth;
    bool isSet;
    private int heartAmount = 1;

    void Awake(){
        gameController = this.gameObject.GetComponent<GameController>();
        hearts[0] = leftHeart;
        hearts[1] = rightHeart;
    }

    private void Update(){

        currentHealth = gameController.player.GetComponent<PlayerLogic>().currentHealth;
        maxHealth = gameController.player.GetComponent<PlayerLogic>().maxHealth;

        /*
        if(currentHealth <= 0f){
            Time.timeScale = 0;
            gameOverScreen.gameObject.SetActive(true);
        }
        */

        tmpHealth = 0;
        isSet = false;
        for (int i = 0; i < heartAmount; i++){
            if(tmpHealth + 100f < currentHealth){
                hearts[i].SetFullness(HealthFullness.Full);
                tmpHealth += 100f;
            }
            else if(tmpHealth + 100f >= currentHealth && isSet == false) {

                if((currentHealth - tmpHealth)/100f == 1f){
                    //Debug.Log("100");
                    hearts[i].SetFullness(HealthFullness.Full);
                }
                else if((currentHealth - tmpHealth)/100f == 0.75f){
                    //Debug.Log("75");
                    hearts[i].SetFullness(HealthFullness.ThreeQuarters);
                }
                else if((currentHealth - tmpHealth)/100f == 0.5f){
                    //Debug.Log("50");
                    hearts[i].SetFullness(HealthFullness.Half);
                }
                else if((currentHealth - tmpHealth)/100f == 0.25f){
                    //Debug.Log("25");
                    hearts[i].SetFullness(HealthFullness.OneQuarter);
                }
                else if((currentHealth - tmpHealth)/100f == 0f){
                    //Debug.Log("0");
                    hearts[i].SetFullness(HealthFullness.Empty);
                }
                else{
                    //Debug.Log("Current Health: " + currentHealth);
                    //Debug.Log("Nem állitottam semmit");
                }
                isSet = true;
            }
            else{
                hearts[i].SetFullness(HealthFullness.Empty);
            }
        }

        if(Input.GetKeyDown("p")){
            Debug.Log("Sebeztem 25-öt");
            gameController.player.GetComponent<PlayerLogic>().GetHit(new Vector3(0,0,0),25f);
            Debug.Log("Enny HP maradt: " + gameController.player.GetComponent<PlayerLogic>().currentHealth);
        }
    }

}
