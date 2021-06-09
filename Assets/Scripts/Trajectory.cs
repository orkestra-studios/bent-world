using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Vectors;

public class Trajectory : MonoBehaviour {

    [Range(1,1000)] public int resolution = 24;
    public Bow bow;
    public LineRenderer renderer;
    public float duration = 10f;

    public List<Vector3> points;

    void Start() {
        
    }

    void Generate(Vector3 aim, float power) {

        Vector3 velocity = (power * aim) * Time.fixedDeltaTime; // TODO: arrow mass
        float g = Physics.gravity.y;
        float timeStep = duration / resolution;

        points.Clear();
        for (int i = 0; i < duration; i++) {
            float t = timeStep * i;
            Vector3 movement = velocity * t + (g * t * t / 2).Y();
            points.Add(transform.position+movement);
        }

        if(points.Count < 1) return;

        renderer.positionCount = points.Count;
        renderer.SetPositions(points.ToArray());
    }

    void FixedUpdate() {
        if(bow.state == Bow.State.Aiming) {
            Generate(bow.direction, bow.power);
            renderer.enabled = true;
        } else {
            renderer.enabled = false;
        }
    }
}
