using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Utility.Vectors;

public class Flapper : MonoBehaviour {

    public Rigidbody body;
    [Range(0,100)] public float speed = 3f;
    [Range(0,1)] public float slomo = 0.5f; 
    public float movement = 0.25f;

    float _speed, _slomo;

    void Update() {
        _slomo = Mathf.Lerp(_slomo, MobileInput.Touching ? (1-slomo) : 1, 0.2f);
        if(MobileInput.TouchUp) Flap();
        if(MobileInput.Touching) {
            float dt = _slomo * Time.fixedDeltaTime;
            Physics.autoSimulation = false;
            Physics.Simulate(dt);
        }
    }

    void Flap() {
        Physics.autoSimulation = true;
        float h = Mathf.Clamp(transform.position.y, 1, 2);
        body.velocity = Vector3.Slerp(Vector3.up, transform.forward, movement) * speed / h;
    }

}
