using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = this.transform.position;
    }
}
