using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject replicator;

    public void RefreshPlayer(){
        player = GameObject.Find("Player");
        replicator.GetComponent<Replicator>().CreateNewPlayer();
    }
}
