using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyParallax : MonoBehaviour
{
    public List<Transform> backgroundLayers;
    public Transform foregroundLayer;
    public Transform target;
    public bool hasForegorund = true;

    [SerializeField] private Vector2 parallaxEffectMultiplier;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start(){
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate(){
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y,0f);
        lastCameraPosition = cameraTransform.position;
    }
}
