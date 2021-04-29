using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR 
    using UnityEditor;

    [CustomEditor(typeof(FollowerConfig))]
    public class FollowerConfigEditor : Editor {
        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            FollowerConfig config = (FollowerConfig)target;

            EditorGUILayout.Space();
            if(GUILayout.Button("Synchronize", GUILayout.MinWidth(100), GUILayout.Height(30))) {
                config.Synchronize();
            }
        }
    }
#endif

[CreateAssetMenu(fileName = "Follow", menuName = "Configuration/Follower", order = 1)]
public class FollowerConfig : ScriptableObject {

    [Header("Movement")]
    public Vector3 movementSpeed;
    public Vector3 positionOffset;
    public bool movementIsRelative = false;

    [Header("Rotation")]
    public Vector3 rotationSpeed;
    public Vector3 rotationOffset;
    public bool rotationIsRelative = false;

    [Space]

    public float dynamicSpeed = 1;
    public bool transformLocally = false;

    public void Synchronize() {
        foreach(Follower f in FindObjectsOfType<Follower>()){
            if(f.config == this) f.ApplyTransform();
        }
    }

}
