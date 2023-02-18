using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum NextWorldEnterSide{
    Left,
    Left_2,
    Right,
    Right_2,
    Up,
    Up_2,
    Down,
    Down_2
}

public class SceneChanger : MonoBehaviour
{
    public string nextScene;
    public NextWorldEnterSide nextWorldEnterSide;
    public GameController gameController;

    void Awake(){
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    void OnTriggerEnter2D(Collider2D collider){
        gameController.nextWorldEnterSide = nextWorldEnterSide;
        SceneManager.LoadScene(nextScene);
    }
}
