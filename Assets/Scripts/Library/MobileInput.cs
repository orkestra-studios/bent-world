using UnityEngine;

public enum Direction {
    Right = 0,
    Up = 90,
    Left = 180,
    Down = 270,
    All = 360
}

public interface InputSource {
    
}

public class MobileInput : MonoBehaviour  {

    private static bool _touchDown, _touchUp, _touching, _moving;
    private static Vector2 _anchor, _rel, _pos, _dpos;

    private static Vector2 _screen;
    private static float _timer;

    public bool enableDeviceScreen;

    void Start() {
        _screen = new Vector2(Screen.width, Screen.height);
        _pos = _screen / 2;
        _dpos = _pos;
        _anchor = _pos;
        _rel = _pos;
        _timer = 0;
    }

    void Update() {
        #if UNITY_EDITOR 
            if(enableDeviceScreen) DeviceHandler();
            else                   EditorHandler();
        #elif UNITY_IOS
            DeviceHandler();
        #endif
    }

    private static void DeviceHandler() {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    _timer = 0;
                    _rel = touch.position;
                    UpdateValues(true, false, true, false, touch.position, touch.position);
                    break;

                case TouchPhase.Moved:
                    UpdateValues(true, true, false, false, touch.position, _anchor);
                    break;

                case TouchPhase.Stationary:
                    UpdateValues(true, false, false, false, touch.position, _anchor);
                    break;

                case TouchPhase.Ended:
                    UpdateValues(false, false, false, true, touch.position, _anchor);
                    break;

                case TouchPhase.Canceled:
                    UpdateValues(false, false, false, false, touch.position, touch.position);
                    break;
            }
            _timer += Time.deltaTime;
        } else {
            UpdateValues(false, false, false, false, _pos, _anchor);
        }
        _rel = Vector2.Lerp(_rel, _pos, 0.1f);
    }

    private static void UpdateValues(bool __touching, bool __moving, bool __touchDown, bool __touchUp, Vector2 __pos, Vector2 __anchor) {
        _touching  = __touching;
        _moving    = __moving;
        _touchDown = __touchDown;
        _touchUp   = __touchUp;
        _pos       = __pos;
        _anchor    = __anchor;
    }

    private static void EditorHandler()  {
        _touchDown = Input.GetMouseButtonDown(0);
        _touchUp = Input.GetMouseButtonUp(0);

        if(_touchDown) {
            _timer = 0;
            _anchor = Input.mousePosition;
            _pos = _anchor;
            _dpos = _pos;
            _rel = _anchor;
            _moving = false;
        }

        if (Input.GetMouseButton(0)) {
            _touching = true;
            _pos = Input.mousePosition;
            _timer += Time.deltaTime;
            _moving = (_pos-_dpos).sqrMagnitude > 0.1f;
            _rel = Vector2.Lerp(_rel, _pos, 0.1f);
        } else {
            _touching = false;
            _moving = false;
        }
    }

    public static Vector2 Displacement {
        get { return _pos - _anchor; }
    }

    public static Vector2 Movement {
        get { return _pos - _rel; }
    }

    public static bool Touching {
        get { return _touching; }
    }

    public static bool Moving {
        get { return _moving; }
    }

    public static bool TouchDown {
        get { return _touchDown; }
    }

    public static bool TouchUp {
        get { return _touchUp; }
    }

    public static Vector2 TouchPoint {
        get { return _pos; }
    }

    public static Vector2 AnchorPoint {
        get { return _anchor; }
    }

    public static Vector2 RelativePoint {
        get { return _rel; }
    }

    public static Vector2 ScreenSize {
        get { return _screen; }
    }

    public static float TotalTime {
        get { return _timer; }
    }

    public static bool Tap {
        get { return _touchUp && _timer < 0.2f && Displacement.sqrMagnitude < 25f; }
    }

    public static bool Swipe(Direction direction = Direction.All) {
        if(_timer > 0.5f) return false;
        if(!_touchUp) return false;
        Vector2 movement = _pos - _anchor;
        if(movement.sqrMagnitude < 10000) return false;
        if(direction == Direction.All) return true;
        float angle = Mathf.Repeat(360 + Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg, 360);
        return Mathf.Abs(angle - (int)direction) < 30 || Mathf.Abs(angle - (int)direction) > 330;
    }

}
