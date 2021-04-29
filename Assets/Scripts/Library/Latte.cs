using System;
using UnityEngine;

public class Latte : MonoBehaviour {

    private static Action next;
    private static uint priority = 0;
    [Range(1,50)] public float frequency = 1;
    private float cnt = 0;

    void Update() {
        if(cnt <= 0 && next != null) {
            next();
            next = null;
            priority = 0;
            cnt = 1/frequency;
        } else {
            cnt -= Time.deltaTime;
        }
    }

    public static void Run(Action r, uint p = 1) {
        if(p >= priority) {
            next     = r;
            priority = p;
        }
    }
}