using UnityEngine;

public class MaterialSelector : MonoBehaviour {

    //public Transform parentPrefab;

	[SerializeField]
	Material[] materials = default;
    float duration = 0f;

	[SerializeField]
	MeshRenderer meshRenderer = default;
    bool Gate;

    int fakeIndex = 0;

	public void Select (int index) {
        duration = 0;
		if (
			meshRenderer && materials != null &&
			index >= 0 && index < materials.Length
		) {
            fakeIndex = index;
            Gate = true;
            
		}
	}
    void Update() {
        if (Gate){
            if (duration >= 1){
               // Debug.Log("full duration" + duration);
                duration = 0;
                Gate = false;
            }
            if (duration < 1){
              //  Debug.Log("Lerping..." + duration);
                duration +=  1f; //.01f;
                meshRenderer.material.Lerp (meshRenderer.material, materials[fakeIndex], duration);
                
            }

        }
    }
}