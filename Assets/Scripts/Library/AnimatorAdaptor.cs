using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Vectors;

public class AnimatorAdaptor : MonoBehaviour {

    public Movement movement;
    public Animator animator;

    [SerializeField] Vector3 v;

    void Update() {
        Vector3 cn = transform.InverseTransformDirection(movement.direction); 
        v = cn * movement._speed / 3;
        animator.speed = 1;
        animator.SetFloat("vx",v.x);
        animator.SetFloat("vy",v.z);
    }
}