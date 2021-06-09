using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Vectors;

public class Movement : MonoBehaviour {

    //parameters
    [Header("Parameters")]
    [Range(1,20)]  public float speed = 1;
    public bool rotates = false;
    public bool grounded = false;

    [Space]
    public Rigidbody body;
    
    public float _speed {
        get; protected set;
    }

    public Vector3 direction {
        get; protected set;
    }

    protected virtual void HandleInput() {}

    void Update() => HandleInput();
    
    void FixedUpdate() {
        body.velocity = direction * _speed + body.velocity.Y();
        if(rotates) {
            body.rotation = Quaternion.LookRotation(
                Vector3.Lerp(transform.forward, direction, Time.smoothDeltaTime * _speed)
            );
        }
        
        
    }

    Vector3 Ground(Vector3 f) {
        Ray ray = new Ray(transform.position, Physics.gravity);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 3f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore)) {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            return slopeRotation * f;
        } else return f;

    }

}
