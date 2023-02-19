using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public List<Transform> backgroundLayers;
    public Transform foregroundLayer;
    public Transform target;
    public Vector3 spawnPosition;
    public bool hasForegorund = true;
    public Vector3 calculationPos;
    public Vector3 offSet;
    public Vector3 targetPos;

    void Start(){
        spawnPosition = GameObject.Find("Camera").transform.position;
        offSet = spawnPosition;
    }

    void Update()
    {
        if(target == null){
            target = GameObject.Find("Camera").transform;
        }

        calculationPos = target.position - offSet;
        targetPos = target.position;

        

        for (int i = 0; i < backgroundLayers.Count; i++)
        {
            float div = (float)(i + 2);
            Vector3 position = calculationPos / div;
            position.z = backgroundLayers[i].position.z;
            backgroundLayers[i].position = position + offSet;
        }

        if(hasForegorund)
        {
            Vector3 position = calculationPos / 50f;
            position.z = foregroundLayer.position.z;
            position.y = position.y-2f;
            foregroundLayer.position = position + offSet;
        }
        

        /*
        for (int i = 0; i < backgroundLayers.Count; i++)
        {
            float div = (float)(i + 2);
            Vector3 position = target.position / div;
            position.z = backgroundLayers[i].position.z;
            backgroundLayers[i].position = position;
        }

        if(hasForegorund)
        {
            Vector3 position = target.position * -0.5f;
            position.z = foregroundLayer.position.z;
            foregroundLayer.position = position;
        }
        */
        
    }
}
