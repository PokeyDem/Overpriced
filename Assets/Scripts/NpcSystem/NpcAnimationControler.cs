using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAnimationControler : MonoBehaviour {
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("Walk", (agent.velocity.magnitude > 0.01f));
    }
}
