using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{   
    public AudioClip keyPickupSound;
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            coll.GetComponent<PlayerVariables>().TogglePlayerKey();
            GameManager.Instance.playerSpeaker.PlayOneShot(keyPickupSound);
            Destroy(this.gameObject);
        }
    }
}
