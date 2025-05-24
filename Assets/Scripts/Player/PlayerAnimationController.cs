using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerControl playerControl;
    
    void Update()
    {
        animator.SetBool("Walk", playerControl.IsMoving());
    }
}
