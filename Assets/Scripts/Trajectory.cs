using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Vectors;

public class Trajectory : MonoBehaviour
{

    public enum Render {Always, OnInput}
    public Render render = Render.Always;
    [Range(1, 100)] public int resolution = 24;
    public Bow bow;
    public LineRenderer _renderer;
    [Range(1, 5)] public float duration = 1f;

    public List<Vector3> points;

    void Generate(Vector3 aim, float power, float mass = 1) {
        //             v = ƒ             * t                   / m
        Vector3 velocity = (power * aim) * Time.fixedDeltaTime / mass;
        Vector3 g = Physics.gravity.Y();
        float timeStep = 1f/resolution;
        float sampleCount = duration * resolution;

        points.Clear();
        for (int i = 0; i < sampleCount; i++) {
            float t = timeStep * i;
            Vector3 movement = velocity * t + (g * t * t / 2);
            points.Add(transform.position + movement);
        }
        if (points.Count < 1) return;

        _renderer.positionCount = points.Count;
        _renderer.SetPositions(points.ToArray());
    }

    void FixedUpdate() {
        switch (render) {
            case Render.Always:
                Generate(bow.direction, bow.power, bow.arrowMass);
                break;
            case Render.OnInput:
                if(MobileInput.Touching) {
                    Generate(bow.direction, bow.power, bow.arrowMass);
                    _renderer.enabled = true;
                } else {
                    _renderer.enabled = false;
                }
                break;
        }
    }
}
