using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SecurityGuardAnimController))]
public class customInspectorSec : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SecurityGuardAnimController security = (SecurityGuardAnimController)target;
        if(GUILayout.Button("Set Happy")){
            security.setHappy();
        }
        if(GUILayout.Button("Set Sad")){
            security.setSad();
        }
        if(GUILayout.Button("Set Base")){
            security.setBase();
        }
        if(GUILayout.Button("Set Scared")){
            security.setScared();
        }
        if(GUILayout.Button("Set Angry")){
            security.setAngry();
        }
        if(GUILayout.Button("Set Trance")){
            security.setTrance();
        }
    }
}
