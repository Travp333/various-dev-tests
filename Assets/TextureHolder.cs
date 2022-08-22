using UnityEngine;
[CreateAssetMenu]
public class TextureHolder : ScriptableObject {

	[SerializeField]
	Texture[] tex;
    public Texture getTex(int texID){
        if(texID == 0){
            return tex[0];
        }
        else if (texID == 1){
            return tex[1];
        }
        else{
            return tex[2];
        }

    }

}