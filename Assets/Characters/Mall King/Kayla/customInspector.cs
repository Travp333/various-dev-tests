using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KaylaFaceAnimController))]
public class customInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KaylaFaceAnimController kayla = (KaylaFaceAnimController)target;
        if(GUILayout.Button("Set Happy")){
            kayla.setHappy();
        }
        if(GUILayout.Button("Set Sad")){
            kayla.setSad();
        }
        if(GUILayout.Button("Set Base")){
            kayla.setBase();
        }
        if(GUILayout.Button("Set Scared")){
            kayla.setScared();
        }
        if(GUILayout.Button("Set Angry")){
            kayla.setAngry();
        }
        if(GUILayout.Button("Set Trance")){
            kayla.setTrance();
        }
    }
}
