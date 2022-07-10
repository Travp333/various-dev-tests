using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSelector : MonoBehaviour {

    //public Transform parentPrefab;

	[SerializeField]
	Material[] materials = default;
    //float duration = 0f;

	[SerializeField]
	MeshRenderer[] meshRenderers = default;
    [SerializeField]
    SkinnedMeshRenderer[] skinnedMeshRenderers = default;
    [SerializeField]
    bool isSkinned;
    [SerializeField]
    public List<MaterialSelector> matsec = new List<MaterialSelector>();
    bool Gate;

    //int fakeIndex = 0;

	public void Select (int index) {
        if(!isSkinned){
            //duration = 0;
            foreach (MeshRenderer m in meshRenderers){
                if (
                    m && materials != null &&
                    index >= 0 && index < materials.Length
                ) {
                    m.material = materials[index];
                    //fakeIndex = index;
                    // Gate = true;
                    
                }
            }
        }
        else{
            foreach(SkinnedMeshRenderer s in skinnedMeshRenderers){
                if (
                    s && materials != null &&
                    index >= 0 && index < materials.Length
                ) {
                    s.material = materials[index];
                    //fakeIndex = index;
                    //Gate = true;
                    
                }
            }
        }
        if(matsec.Count > 0){
            foreach (MaterialSelector m in matsec){
                m.Select(index);
            }
        }
	}
    //void Update() {
    //    if (Gate){
    //        if (duration >= 1){
    //           // Debug.Log("full duration" + duration);
    //            duration = 0;
    //            Gate = false;
    //        }
    //        if (duration < 1){
    //          //  Debug.Log("Lerping..." + duration);
    //            duration +=  1f; //.01f;
    //            meshRenderer.material.Lerp (meshRenderer.material, materials[fakeIndex], duration);
                
    //        }

    //    }
   // }
}