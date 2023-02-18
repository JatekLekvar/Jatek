using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject replicator;

    public NextWorldEnterSide nextWorldEnterSide;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("lobby");
    }

    public void RefreshPlayer(){
        player = GameObject.Find("Player");
        replicator.GetComponent<Replicator>().CreateNewPlayer();
    }


}
