using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public NextWorldEnterSide NextWorldEnterSide;
    public GameController gameController;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        if(gameController.nextWorldEnterSide == this.NextWorldEnterSide){
            player.transform.position = this.transform.position;
        }
    }
}
