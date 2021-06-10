using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using Utility.Vectors;

public class Archer : MonoBehaviour {

    public Rigidbody body;
    public Bow bow;

    public VolumeProfile profile;
    public Vignette vignette;
    public ChromaticAberration aberration;

    [Range(1,10)] public float flipPower;
    [Range(1,10)] public float slomoPower;

    float jump = 1;

    void Start() {
        profile.TryGet<Vignette>(out vignette);
        profile.TryGet<ChromaticAberration>(out aberration);
    }

    public void Flip() {
        body.constraints = RigidbodyConstraints.FreezePositionX;// | ~RigidbodyConstraints.FreezeRotationX;
        float r05_1 = Random.value/2 + 0.5f;
        float r05_2 = Random.value/2 + 4f;
        Vector3 flip = (Vector3.forward * r05_1 + Vector3.up * r05_2) * flipPower * jump * bow.power / 1000;
        body.velocity = body.velocity.Z()/2;
        body.AddForce(flip, ForceMode.Impulse);
        body.AddTorque(Vector3.left * flipPower * 100, ForceMode.Impulse);
        jump *= 0.9f;
    }

    void FixedUpdate() {
        if(bow.state == Bow.State.Aiming) {
            Physics.autoSimulation = false;
            Physics.Simulate(Time.fixedDeltaTime/slomoPower);
            Level.current.camera.positionOffset = Level.current.camera.config.positionOffset / 8;
            Level.current.camera.rotationOffset = bow.transform.forward/10;

            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.2f, 0.1f);
            aberration.intensity.value = Mathf.Lerp(aberration.intensity.value, 0.2f, 0.1f);
        } else {
            Physics.autoSimulation = true;
            Level.current.camera.positionOffset = Vector3.zero;
            Level.current.camera.rotationOffset = Vector3.zero;
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, 0.25f);
            aberration.intensity.value = Mathf.Lerp(aberration.intensity.value, 0f, 0.25f);
        }
    }

    private void OnCollisionEnter(Collision other) {
        jump = 1;
        body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
    }
}
