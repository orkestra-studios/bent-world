using UnityEngine;

using Utility.Channel;

public enum ControlState { Controlable, AI, Ignore }

public enum Type  { Red, Blue }

public class Level : MonoBehaviour {

    public static Level current {get; private set;}

    public enum GameState {Started, Playing, Failed, Finished}

    public int number;
    public GameState state = GameState.Started;
    public new Follower camera;

    public bool debugging = false;

    public UI ui;

    void Awake() {
        current = this;
        number = PlayerPrefs.GetInt(Loader.k_Level, 1);
        Application.targetFrameRate = 60;
        ui.Setup(number);
        if(debugging) Channel.Setup();
        Channel.Reset();
        //TinySauce.OnGameStarted(number.ToString());
    }

    void Update() {

        switch (state) {

            case GameState.Playing: {

            break; }

            case GameState.Finished: {
            break; }

            case GameState.Failed: {
            break; }

        }
    }

    public void Reload() {
        Loader.Reload(debugging);
    }

    public void Finish() {
        if(state != GameState.Playing) return;
        Debug.Log("FINISHED");
        state = GameState.Finished;
        Loader.Next(number+1);

        ui.ShowMessage("YOU WON!");
        
        HapticEngine.NotificationFeedbackSuccess();
        Channel.Broadcast("Level.Finished");
        //TinySauce.OnGameFinished(number.ToString(), true, 0);
    }

    public void Fail() {
        if(state != GameState.Playing) return;
        state = GameState.Failed;

        Loader.Next(number);
        ui.ShowMessage("YOU LOST!");

        HapticEngine.NotificationFeedbackError();
        Channel.Broadcast("Level.Failed");
        //TinySauce.OnGameFinished(number.ToString(), false, 0);
    }

    void OnApplicationQuit() {
        PlayerPrefs.SetInt("checkpoint", -1);
    }

    public void UpdateCamera(Transform ft, Transform lt, FollowerConfig conf = null) {
        camera.config = conf ?? camera.config;
        camera.Follow(ft);
        camera.LookAt(lt);
    }
}
