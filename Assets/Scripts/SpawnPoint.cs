using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public NextWorldEnterSide worldEnterSide;
    public GameController gameController;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        if(gameController.nextWorldEnterSide == this.worldEnterSide){
            player.transform.position = this.transform.position;
        }
    }
}
