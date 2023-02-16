using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string nextScene;

    void OnTriggerEnter2D(Collider2D collider){
        SceneManager.LoadScene(nextScene);
    }
}
