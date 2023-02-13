using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private Image myImage;
    [SerializeField] private Image inventory;
    [SerializeField] private HealthBarController healthBarController;
    void Update()
    {
        if(healthBarController.currentHealth <= 0f){
            Time.timeScale = 0;
            inventory.gameObject.SetActive(false);   
            healthBarController.gameOverScreen.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenuOpenClose();
        }

        if(Input.GetKeyDown("i") && Time.timeScale == 1)
        {
            if(inventory.IsActive()){
                inventory.gameObject.SetActive(false);   
            }
            else{
                inventory.gameObject.SetActive(true);   
            }
        }
    }

    public void InGameMenuOpenClose()
    {
        if(Time.timeScale == 1f){
            Time.timeScale = 0f;
            myImage.gameObject.SetActive(true);
        }
        else{
            Time.timeScale = 1f;
            myImage.gameObject.SetActive(false);
        }
    }

    public void OnClickContinue()
    {
        InGameMenuOpenClose();
    }

}
