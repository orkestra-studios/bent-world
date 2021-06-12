using UnityEngine;

using Utility.Vectors;

public class Bow : MonoBehaviour {

    public enum Shooting {
        Auto, Manual
    }

    public enum State {
        Loading,
        Loaded,
        Fired
    }

    public State state = State.Fired;
    public Shooting shooting = Shooting.Auto;

    [Space]

    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float sensitivity = 1;
    [SerializeField] float cooldownTime = 1;
    [SerializeField] float _power = 4;
    [SerializeField] Vector2 range;

    [Space]
    public BowString bowString;
    public Transform arrowFeed;
    public Vector3 direction { get; private set; }
    public float arrowMass {
        get { return arrow ? arrow.body.mass : 1; }
    }

    public float power { get; private set; }
    float angle, cooldown = 0;
    bool shouldShoot = false;
    Arrow arrow;
    
    void Awake() {
        if(shooting == Shooting.Auto) Load();
    }

    void Aim() {
        Vector2 movement = MobileInput.Movement.Divide(MobileInput.ScreenSize) * sensitivity;
        angle = Mathf.Clamp(angle - Mathf.Asin(movement.y) * Mathf.Rad2Deg, -range.x, -range.y);
        power = _power*150 + (angle-15) * -_power;
        direction = Quaternion.Euler(angle.X())*Vector3.forward;
        Debug.DrawRay(transform.position, direction*2, Color.red);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Load() {
        if(state == State.Fired) {
            state = State.Loading;
            arrow = GameObject
                .Instantiate(arrowPrefab, arrowFeed.position, arrowFeed.rotation, transform)
                .GetComponent<Arrow>();
            arrow.transform.localScale = 1.XY() + 0.Z();
            bowString.tail = arrow.tail;
        } else if(state == State.Loading) {
            cooldown -= Time.deltaTime;
            if(shooting == Shooting.Manual || cooldown<=0) {
                AdjustArrow();
                if(arrow.transform.localScale.z>=1) {
                    arrow.transform.localScale = 1.XYZ();
                    cooldown = cooldownTime/2;
                    state = State.Loaded;
                }
                return;
            }
            
        }
    }

    void AdjustArrow() {
        arrow.transform.localPosition = Mathf.Lerp(arrow.transform.localPosition.x, 0.1f, 0.25f).X()
                                      + Mathf.Lerp(arrow.transform.localPosition.y, 0, 0.25f).Y()
                                      + Mathf.Lerp(arrow.transform.localPosition.z, 3.5f-Mathf.Pow(power/(_power*150),2), 0.32f).Z();
        arrow.transform.localRotation = Quaternion.Lerp(arrow.transform.localRotation, Quaternion.identity, 0.4f);
        arrow.transform.localScale    = 1.XY()
                                      + Mathf.Lerp(arrow.transform.localScale.z, 1.05f, 0.36f).Z();
    }

    void Fire() {
        if(state == State.Fired) return;
        cooldown -= Time.deltaTime;
        if(shooting == Shooting.Manual || cooldown<=0) {
            cooldown = cooldownTime/2;
            bowString.tail = null;
            arrow.Shoot(direction, power);
            state = State.Fired;
            shouldShoot = false;
        }
    }

    void Update() {
        switch (shooting) {
            case Shooting.Auto:   AutoShoot(); break;
            case Shooting.Manual: ManualShoot(); break;
        }
    }

    void AutoShoot() {
        if(arrow && MobileInput.Touching) Aim();
        switch (state) {
            case State.Loaded: Fire(); break;
            default:           Load(); break;
        }
    }

    void ManualShoot() {
        if(MobileInput.Touching) Aim();
        if(MobileInput.TouchUp) shouldShoot = true;
        switch (state) {
            case State.Loaded: 
                AdjustArrow();
                if(shouldShoot) Fire();
                break;
            case State.Fired: 
                if(MobileInput.TouchDown) Load();
                break;
            case State.Loading: 
                Load(); 
                break;
        }
    }
}
