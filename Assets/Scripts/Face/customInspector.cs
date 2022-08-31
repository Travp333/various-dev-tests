using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FaceTexController))]
public class customInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FaceTexController face = (FaceTexController)target;
        if(GUILayout.Button("Set Happy")){
            face.setHappy();
        }
        if(GUILayout.Button("Set Sad")){
            face.setSad();
        }
        if(GUILayout.Button("Set Base")){
            face.setBase();
        }
        if(GUILayout.Button("Set Scared")){
            face.setScared();
        }
        if(GUILayout.Button("Set Angry")){
            face.setAngry();
        }
        if(GUILayout.Button("Set Trance")){
            face.setTrance();
        }
    }
}
