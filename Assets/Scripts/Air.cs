using UnityEngine;

[CreateAssetMenu(fileName = "Air", menuName = "Configuration/Air", order = 0)]
public class Air : ScriptableObject {

    public Vector3 draft;
    public Vector3 turbulence;
    
    public Vector3 velocityOn(Vector3 pos) {
        return Vector3.zero;
    }

}
