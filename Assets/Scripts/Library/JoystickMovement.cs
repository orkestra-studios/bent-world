using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Vectors;

public class JoystickMovement : Movement {

    [Range(0.25f,2)] public float sensitivity = 1;

    public Vector3 velocity {
        get { return body.velocity; }
    }

    protected override void HandleInput() {
        if(MobileInput.Touching) {
            if(MobileInput.Displacement.sqrMagnitude>1) {
                Vector3 disp = Vector3.ClampMagnitude(MobileInput.Displacement.V3D() * sensitivity / 40, 1);
                direction = Vector3.Slerp(
                    direction, 
                    disp.normalized,
                    0.25f
                );
                _speed = disp.magnitude * speed; 
            }
        } else {
            _speed = Mathf.Lerp(_speed, 0, 0.25f); 
        }
    }
}
