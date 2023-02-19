using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject replicator;
    public GameObject spaceShipEntrance;

    public NextWorldEnterSide nextWorldEnterSide;
    public bool testing;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(!testing){
            nextWorldEnterSide = NextWorldEnterSide.Up;
            SceneManager.LoadScene("Space Ship");
        }
    }

    void Update(){
        spaceShipEntrance = GameObject.Find("Space Ship Entrance");
    }

    public void RefreshPlayer(){
        player = GameObject.Find("Player");
        nextWorldEnterSide = NextWorldEnterSide.Up;
        SceneManager.LoadScene("Space Ship");
        replicator.GetComponent<Replicator>().CreateNewPlayer();
    }

}
