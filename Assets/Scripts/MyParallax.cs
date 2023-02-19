using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParallax : MonoBehaviour
{
    //public List<Transform> backgroundLayers;
    //public Transform foregroundLayer;
    //public Transform target;
    //public bool hasForegorund = true;

    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private SceneSpecs sceneSpecs;
    private float fixY;

    private void Start(){
        fixY = this.transform.position.y;
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        sceneSpecs = GameObject.Find("Scene Specs").GetComponent<SceneSpecs>();
    }



    private void LateUpdate(){
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, (sceneSpecs.maxY + sceneSpecs.minY)/2,0f);
        transform.position = new Vector3(transform.position.x, fixY,transform.position.z);
        lastCameraPosition = cameraTransform.position;
    }
}
