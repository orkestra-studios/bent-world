using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    [SerializeField] ParticleSystem pop;
    public Air air;

    public void Pop() {
        pop.transform.parent = null;
        pop.Play();
        Destroy(gameObject);
    }

}
