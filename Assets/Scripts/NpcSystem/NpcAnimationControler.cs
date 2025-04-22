using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAnimationControler : MonoBehaviour {
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    
    // Update is called once per frame
    void Update() {
        animator.SetBool("Walk", (agent.velocity.magnitude > 0.01f));
    }
}
