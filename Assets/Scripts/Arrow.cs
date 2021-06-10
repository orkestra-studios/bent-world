using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public Transform tail;
    [SerializeField] TrailRenderer trail;
    [SerializeField] Rigidbody body;

    void Awake() {
        body.isKinematic = true;
        enabled = false;
        trail.enabled = false;
    }

    public void Shoot(Vector3 direction, float power) {
        transform.parent = null;
        body.isKinematic = false;
        trail.enabled = true;
        transform.localScale = Vector3.one/4;
        enabled = true;
        float dt = Time.fixedDeltaTime;
        body.AddForce(direction*power*dt, ForceMode.Impulse);
    }   

    void FixedUpdate() {
        body.rotation = Quaternion.LookRotation(body.velocity);
    }

    private void OnCollisionEnter(Collision other) {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        //body.isKinematic = true;
        if(other.gameObject.layer == 7) {
            other.gameObject.GetComponent<Balloon>().Pop();
        }
    }

}
