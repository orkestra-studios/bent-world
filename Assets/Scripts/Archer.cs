using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Utility.Vectors;

public class Archer : MonoBehaviour {

    public Rigidbody body;
    public Bow bow;
    [Range(0,10)] public float speed = 3f;
    [Range(0,1)] public float slomo = 0.5f; 

    float _speed, _slomo;

    void FixedUpdate() {
        float dt = Time.smoothDeltaTime;
        _slomo = Mathf.Lerp(_slomo, MobileInput.Touching ? (1-slomo) : 1, 0.2f);
        transform.position += (speed * _slomo * dt).Z();
    }

}
