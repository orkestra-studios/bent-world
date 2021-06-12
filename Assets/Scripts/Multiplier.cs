using UnityEngine;

public class Multiplier : MonoBehaviour {

    [SerializeField] int multiplier = 2;
    [SerializeField] TextMesh label;

    void Start() {
        label.text = multiplier.ToString("x#");
    }

    void OnTriggerEnter(Collider other) {

    }

}
