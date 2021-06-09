using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour {
    
    public Transform top, tail, bottom;
    public LineRenderer _renderer;

    // Update is called once per frame
    void Update() {
        if(tail) {
            float tailPos = transform.parent.InverseTransformPoint(tail.position).z;
            if(tailPos <= top.localPosition.z) {
                _renderer.positionCount = 3;
                _renderer.SetPositions(new Vector3[] { top.position, tail.position, bottom.position });
            } else {
                _renderer.positionCount = 2;
                _renderer.SetPositions(new Vector3[] { top.position, bottom.position });
            }
        } else {
            _renderer.positionCount = 2;
            _renderer.SetPositions(new Vector3[] { top.position, bottom.position });
        }
    }
}
