using UnityEngine;
using Utility.Vectors;

public class Follower : MonoBehaviour {

    public enum State { Waiting, Following } // TODO: position only and look at only states
    public enum UpdatePhase { Normal, Late, Fixed }

    public State state = State.Waiting;
    public UpdatePhase phase = UpdatePhase.Normal;
    public FollowerConfig config;
    public Transform followTarget, lookAtTarget;
    public Rigidbody body;
    public Vector3 positionOffset, rotationOffset;
    private Vector3 startRot;
    private Vector3 affectedVelocity;
    private Vector3 _positionOffset, _rotationOffset;
    private float speedCoeff = 0;
    
    void Start() {
        startRot = transform.rotation.eulerAngles;
        _positionOffset = positionOffset;
        _rotationOffset = rotationOffset;
        //ApplyTransform();
    }

    public void Follow(Transform t) {
        followTarget = t;
        state = State.Following;
    }

    public void LookAt(Transform t) {
        lookAtTarget = t;
        state = State.Following;
    }

    public void Stop() {
        state = State.Waiting;
    }

    void Update()      { 
        _positionOffset = Vector3.Lerp(_positionOffset, positionOffset, config.dynamicSpeed);
        _rotationOffset = Vector3.Lerp(_rotationOffset, rotationOffset, config.dynamicSpeed);
        if(phase == UpdatePhase.Normal) PerformUpdate(); 
    }
    void LateUpdate()  { if(phase == UpdatePhase.Late)   PerformUpdate(); }
    void FixedUpdate() { if(phase == UpdatePhase.Fixed)  PerformUpdate(); }

    void PerformUpdate() {

        switch (state) {
            case State.Following: speedCoeff = Mathf.Lerp(speedCoeff, 1, 0.2f); break;
            case State.Waiting:   speedCoeff = Mathf.Lerp(speedCoeff, 0, 0.2f); break;
        }

        if (body == null) {
            if(config.transformLocally) MoveTransformLocally();
            else                        MoveTransform();
            RotateTransform();
        } else {
            MoveRigidbody();
            RotateRigidBody();
        }

    }

    //TODO: Fix rigidbody system
    public void ResetEffects() {
        if (body != null) {
            affectedVelocity = Vector3.zero;
        }
    }

    public void VelocityEffect(Vector3 effect) {
        if (body != null) {
            affectedVelocity += effect;
        }
    }

    public Vector3 GetEffect() {
        return affectedVelocity;
    }

    void MoveRigidbody() {
        if(followTarget == null) return;
        float dt = Time.deltaTime * speedCoeff;

        Vector3 targetPos = followTarget.transform.position;
        Vector3 oldPos = transform.position;
        Vector3 newPos = targetPos + ((config.movementIsRelative ? followTarget.transform.rotation : Quaternion.identity) * (config.positionOffset + _positionOffset));
        Vector3 dir   = (newPos - oldPos).Map((float f) => Mathf.Clamp(f / 2,-1 , 1));

        Vector3 nv = dir.Multiply(config.movementSpeed + affectedVelocity / 2) + body.velocity.Y();
        body.velocity = Mathf.SmoothStep(body.velocity.x, nv.x, 0.24f).X() + nv.YZ() + affectedVelocity;
        affectedVelocity = Vector3.Lerp(affectedVelocity, Vector3.zero, dt);

    }

    void MoveTransform() {
        if(followTarget == null) return;
        float dt = Time.deltaTime * speedCoeff;
        //movement
        Vector3 targetPos = followTarget.transform.position;
        if((targetPos - transform.position).sqrMagnitude < 16) return;
        Vector3 oldPos = transform.position;
        Vector3 newPos = (targetPos + (config.movementIsRelative ? followTarget.transform.rotation : Quaternion.identity) * (config.positionOffset + _positionOffset));
        transform.position = new Vector3 (
            Mathf.Lerp(oldPos.x, newPos.x, config.movementSpeed.x * dt),
            Mathf.Lerp(oldPos.y, newPos.y, config.movementSpeed.y * dt),
            Mathf.Lerp(oldPos.z, newPos.z, config.movementSpeed.z * dt)
        );
    }

    void MoveTransformLocally() {
        if(followTarget == null) return;
        float dt = Time.deltaTime * speedCoeff;
        //movement
        Vector3 targetPos = followTarget.transform.position - transform.parent.position;
        Vector3 oldPos = transform.localPosition;
        Vector3 newPos = targetPos + config.positionOffset + _positionOffset;
        transform.localPosition = new Vector3 (
            Mathf.Lerp(oldPos.x, newPos.x, config.movementSpeed.x * dt),
            Mathf.Lerp(oldPos.y, newPos.y, config.movementSpeed.y * dt),
            Mathf.Lerp(oldPos.z, newPos.z, config.movementSpeed.z * dt)
        );
    }

    void RotateTransform() {
        if(lookAtTarget == null) return;
        float dt = Time.deltaTime * speedCoeff;
        Vector3 targetPos = lookAtTarget.transform.position;
        Vector3 rotPos = (targetPos + (config.rotationIsRelative ? lookAtTarget.transform.rotation : Quaternion.identity) * (config.rotationOffset + _rotationOffset));
        Vector3 oldDir = transform.rotation.eulerAngles;
        Vector3 diff   = rotPos - transform.position;
        Vector3 newDir = Quaternion.LookRotation(diff).eulerAngles;

        transform.rotation = Quaternion.Euler (
            Mathf.LerpAngle(oldDir.x, newDir.x, config.rotationSpeed.x * dt),
            Mathf.LerpAngle(oldDir.y, newDir.y, config.rotationSpeed.y * dt),
            Mathf.LerpAngle(oldDir.z, newDir.z, config.rotationSpeed.z * dt)
        );
    }

    void RotateRigidBody() {
        if(lookAtTarget == null) return;
        float dt = Time.deltaTime * speedCoeff;
        Vector3 targetPos = lookAtTarget.transform.position;
        Vector3 rotPos = (targetPos + (config.rotationIsRelative ? lookAtTarget.transform.rotation : Quaternion.identity) * (config.rotationOffset + _rotationOffset));
        Vector3 oldDir = transform.rotation.eulerAngles;
        Vector3 diff   = rotPos-transform.position;
        Vector3 newDir = Quaternion.LookRotation(diff).eulerAngles;

        body.rotation = Quaternion.Euler (
            Mathf.LerpAngle(oldDir.x, newDir.x, config.rotationSpeed.x * dt),
            Mathf.LerpAngle(oldDir.y, newDir.y, config.rotationSpeed.y * dt),
            Mathf.LerpAngle(oldDir.z, newDir.z, config.rotationSpeed.z * dt)
        );
    }

    [ContextMenu("Apply Transform")]
    public void ApplyTransform() {
        //Debug.Log("updating follower");
        if(followTarget) {
            Vector3 targetPos = followTarget.transform.position;
            transform.position = (targetPos + (config.movementIsRelative ? followTarget.transform.rotation : Quaternion.identity) * (config.positionOffset + _positionOffset));
        }

        if(lookAtTarget) {
            Vector3 targetPos = lookAtTarget.transform.position;
            Vector3 rotPos = (targetPos + (config.rotationIsRelative ? lookAtTarget.transform.rotation : Quaternion.identity) * (config.rotationOffset + _rotationOffset));
            Vector3 oldDir = transform.rotation.eulerAngles;
            Vector3 diff   = rotPos-transform.position;
            Vector3 newDir = Quaternion.LookRotation(diff).eulerAngles;
            transform.rotation = Quaternion.Euler(newDir);
        }
    }

    public void OnValidate() {
        //if(enabled) ApplyTransform();
    }
}