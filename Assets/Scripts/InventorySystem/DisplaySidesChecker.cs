using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySidesChecker : MonoBehaviour{
    
    // private float offset = 1.0f;
    // private float rayDistance = 2.0f;
    // private LayerMask obstacleLayer;
    //
    // public Vector3 GetApproachPosition(){ //Calculates which on which side of the display npc can approach it
    //     Vector3 front = transform.position + transform.forward * offset;
    //     Vector3 back = transform.position - transform.forward * offset;
    //     Vector3 left = transform.position - transform.right * offset;
    //     Vector3 right = transform.position + transform.right * offset;
    //     obstacleLayer = LayerMask.GetMask("Obstacle");
    //     if (IsSideFree())
    // }
    //
    // public bool IsSideFree(Vector3 position, Vector3 direction)
    // {
    //     Ray ray = new Ray(position, direction);
    //     bool accessible = !Physics.Raycast(ray, rayDistance, obstacleLayer);
    //     return accessible;
    // }
}
