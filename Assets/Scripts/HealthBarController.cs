using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private List<Image> hearts = new List<Image>();
    [SerializeField] public Image gameOverScreen;
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth;
    private float tmpHealth;
    bool isSet;


    private void Update(){

        if(currentHealth <= 0f){
            Time.timeScale = 0;
            gameOverScreen.gameObject.SetActive(true);
        }

        tmpHealth = 0;
        isSet = false;
        for (int i = 0; i < hearts.Count; i++){
            if(tmpHealth + 100f < currentHealth){
                hearts[i].fillAmount = 1f;
                tmpHealth += 100f;
            }
            else if(tmpHealth + 100f >= currentHealth && isSet == false) {
                hearts[i].fillAmount = (currentHealth - tmpHealth)/100f;
                isSet = true;
            }
            else{
                hearts[i].fillAmount = 0f;
            }
        }

        if(Input.GetKeyDown("p")){
            Debug.Log("Sebeztem 25-öt");
            currentHealth -= 25f;
            Debug.Log("Enny HP maradt: " + currentHealth);
        }
    }

}
