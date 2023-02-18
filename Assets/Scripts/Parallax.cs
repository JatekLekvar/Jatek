using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public List<Transform> backgroundLayers;
    public Transform foregroundLayer;
    public Transform target;
    public Vector3 spawnPosition;

    void Update()
    {
        if(target == null){
            target = GameObject.Find("Camera").transform;

        }

        for (int i = 0; i < backgroundLayers.Count; i++)
        {
            float div = (float)(i + 2);
            Vector3 position = target.position / div;
            position.z = backgroundLayers[i].position.z;
            backgroundLayers[i].position = position;
        }

        {
            Vector3 position = target.position * -0.5f;
            position.z = foregroundLayer.position.z;
            foregroundLayer.position = position;
        }
    }
}
