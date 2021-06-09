using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour {


    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(0);
        }
    }
}
