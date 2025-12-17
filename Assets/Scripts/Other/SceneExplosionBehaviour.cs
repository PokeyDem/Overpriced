using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneExplosionBehaviour : SingletonWithDestroy<SceneExplosionBehaviour>
{
    [SerializeField] private float explosionForce = 1500f;
    [SerializeField] private float explosionRadius = 50f;
    [SerializeField] private float rotationForce = 1000f;
    [SerializeField] private float upwardsModifier = 5.0f;
    [SerializeField] private Transform shopRoot;
    
    private List<Rigidbody> debris = new List<Rigidbody>();

    public void TriggerExplosion()
    {
        Debug.Log("Start adding");
        
        Transform[] allChildren = shopRoot.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allChildren)
        {
            if (child == shopRoot) continue;
            
            MeshCollider mc = child.GetComponent<MeshCollider>();
            if (mc != null)
            {
                mc.convex = true;
            }
            
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = child.gameObject.AddComponent<Rigidbody>();
            }
            
            rb.isKinematic = true; 
            
            debris.Add(rb);
        }
        Debug.Log(debris.Count);
        foreach (Rigidbody rb in debris)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
                
                rb.AddTorque(Random.insideUnitSphere * rotationForce);
            }
        }
    }
}
