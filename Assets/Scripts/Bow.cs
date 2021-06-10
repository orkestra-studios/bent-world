using UnityEngine;

using Utility.Vectors;

public class Bow : MonoBehaviour {

    public enum State {
        Aiming,
        Fired
    }

    public State state = State.Fired;
    public GameObject arrowPrefab;
    public Archer archer;
    public float sensitivity = 1;
    public float power = 250;
    [Space]
    public String bowString;
    public Vector3 direction { get; private set; }

    float angle, angularSpeed = 0;
    Arrow arrow;
    
    void Start() {
        
    }

    void Aim() {
        if(state == State.Aiming) {
            angularSpeed = Mathf.Lerp(angularSpeed, 1f, 0.05f);
            UpdateDirection();
            arrow.transform.localPosition = 0.1f.X() + Mathf.Lerp(arrow.transform.localPosition.z, 2.4f-power/750, 0.32f).Z();
            arrow.transform.localScale = 1.XY() + Mathf.Lerp(arrow.transform.localScale.z, 1, 0.32f).Z();
        } else {
            state = State.Aiming;
            arrow = GameObject
              .Instantiate(arrowPrefab, transform.position+transform.forward, transform.rotation, transform)
              .GetComponent<Arrow>();
            arrow.transform.localScale = 1.XY() + 0.1f.Z();
            angle = Mathf.Clamp(transform.parent.rotation.x, -85, 85);
            bowString.tail = arrow.tail;
        }
        
    }

    void Fire() {
        if(state == State.Fired) return;
        state = State.Fired;
        bowString.tail = null;
        angularSpeed = 0;
        arrow.Shoot(direction, power);
        archer.Flip();
    }

    void Update() {
        

        switch (state) {
            case State.Aiming:
                Aim();
                if(MobileInput.TouchUp) Fire();
                break;
            case State.Fired:
                if(MobileInput.TouchDown) Aim();
                break;
        }
    }

    void UpdateDirection() {
        Vector2 movement = MobileInput.Movement.Divide(MobileInput.ScreenSize) * sensitivity;
        angle = Mathf.Clamp(angle - Mathf.Asin(movement.y) * Mathf.Rad2Deg, -85,85);
        power = Mathf.Clamp(power + Mathf.Abs(movement.y) * 500, 400f, 1200f);
        direction = Quaternion.Euler(angle.X())*Vector3.forward;
        Debug.DrawRay(transform.position, direction*2, Color.red);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            angularSpeed
        );
    }
}
