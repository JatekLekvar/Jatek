using System.Collections.Generic;
using UnityEngine;

public class Replicator : MonoBehaviour
{
    public List<string> abilitiesToApply = new List<string>();

    public GameObject nextPlayer;
    public GameObject nextPlayerPrefab;
    public GameObject gameController;

    new public Camera camera;


    void Update()
    {
        /*
        if(nextPlayer == null){
            nextPlayer = (GameObject)Instantiate(nextPlayerPrefab, transform.position, Quaternion.identity);
            nextPlayer.name = "Player";
            nextPlayer.SetActive(false);
        }
        */

        if (Input.GetKeyDown("l"))
        {
            CreateNewPlayer();
        }
    }


    public void CreateNewPlayer()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y + 5,transform.position.z);
        nextPlayer = (GameObject)Instantiate(nextPlayerPrefab, spawnPosition, Quaternion.identity);
        nextPlayer.name = "Player";
        List<GameObject> inReplicatorInventory = gameController.GetComponent<Inventory>().replicatorSlots;

        for (int i = 0; i < inReplicatorInventory.Count; i++)
        {
            if (inReplicatorInventory[i].GetComponent<InventorySlot>().abilityObj == null)
            {
                break;
            }
            abilitiesToApply.Add(inReplicatorInventory[i].GetComponent<InventorySlot>().GetAbility());
        }

        ApplyAllAbilities();

        camera.GetComponent<ScreenCamera>().target = nextPlayer.transform;
        //nextPlayer.SetActive(false);
    }

    public void ApplyAllAbilities()
    {
        PlayerLogic playerLogic = nextPlayer.GetComponent<PlayerLogic>();
        HealthBarController healthBarController = gameController.GetComponent<HealthBarController>();

        for (int i = 0; i < abilitiesToApply.Count; i++)
        {
            switch (abilitiesToApply[i])
            {
                case "speed":
                    playerLogic.RunSpeed = 20f;
                    break;
                case "dmg":
                    playerLogic.damage = 150f;
                    break;
                case "health":
                    healthBarController.heartAmount = 2;
                    playerLogic.maxHealth = 200f;
                    playerLogic.currentHealth = 200f;
                    break;
                default:
                    Debug.Log("Nem volt benne semmi");
                    break;
            }
        }
    }



}
