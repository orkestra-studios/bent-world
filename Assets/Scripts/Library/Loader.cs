using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Utility.Channels;
using Utility.Vectors;

public class Loader : MonoBehaviour {

    public static string k_Level = "level.0.1"; 

    [SerializeField] private int levelCount = 1;

    void Awake() {
        PlayerPrefs.SetInt("checkpoint", PlayerPrefs.GetInt("checkpoint", -1));
        int level = PlayerPrefs.GetInt(k_Level, 1);
        int next = Math.Max(1,level % (levelCount+1));
        Channel.Setup(); 
        SceneManager.LoadScene(next);
    }

    public static void Reload(bool debugging) {
        if (debugging) SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().path);
        else SceneManager.LoadSceneAsync(0);
    }

    public static void Next(int num) {
        PlayerPrefs.SetInt("checkpoint", -1);
        PlayerPrefs.SetInt(Loader.k_Level, num);
    }
}
