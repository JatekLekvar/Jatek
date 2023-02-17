using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image inGameMenuImage;
    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private HealthBarController healthBarController;
    [SerializeField] private GameObject replicator;
    //[SerializeField] private GameObject player;
    [SerializeField] private GameController gameController;
    public List<GameObject> inventorySlots = new List<GameObject>();
    public List<GameObject> replicatorSlots = new List<GameObject>();
    void Update()
    {

        if (healthBarController.currentHealth <= 0f)
        {
            Time.timeScale = 0;
            abilityPanel.SetActive(false);
            healthBarController.gameOverScreen.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenuOpenClose();
        }

        if (Input.GetKeyDown("e") && Time.timeScale == 1 && InRange(replicator, gameController.player))
        {
            abilityPanel.SetActive(!abilityPanel.activeSelf);
        }

        if (!InRange(replicator, gameController.player))
        {
            abilityPanel.SetActive(false);
            abilityPanel.SetActive(false);
        }
    }

    public void InGameMenuOpenClose()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            inGameMenuImage.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            inGameMenuImage.gameObject.SetActive(false);
        }
    }

    public void OnClickContinue()
    {
        InGameMenuOpenClose();
    }


    public bool InRange(GameObject gameObject1, GameObject gameObject2)
    {
        if (Mathf.Abs(gameObject1.transform.position.x - gameObject2.transform.position.x) < 2f && Mathf.Abs(gameObject1.transform.position.y - gameObject2.transform.position.y) < 2f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToInvertory(GameObject gameObject)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].GetComponent<InventorySlot>().abilityObj == null)
            {
                inventorySlots[i].GetComponent<InventorySlot>().abilityObj = gameObject;
                inventorySlots[i].GetComponent<InventorySlot>().UpdateImage();
                break;
            }
        }
    }

    public void AddToReplicator(GameObject gameObject)
    {
        for (int i = 0; i < replicatorSlots.Count; i++)
        {
            if (replicatorSlots[i].GetComponent<InventorySlot>().abilityObj == null)
            {
                replicatorSlots[i].GetComponent<InventorySlot>().abilityObj = gameObject;
                replicatorSlots[i].GetComponent<InventorySlot>().UpdateImage();
                break;
            }
        }
    }

}
