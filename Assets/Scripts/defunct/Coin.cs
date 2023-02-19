// maybe add some up and down movement to the coin?
using UnityEngine;

public class Coin : MonoBehaviour
{
    PlayerStats stats = default;
    public GameObject player = default;

    void Start() {
        stats = player.GetComponent<PlayerStats>();
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            Destroy(gameObject);
            //stats.coins += 1;
        }

    }

}
