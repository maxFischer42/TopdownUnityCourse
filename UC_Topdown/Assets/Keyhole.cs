using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyhole : MonoBehaviour
{

    public GameObject keyIcon;
    public GameObject keyOpenEffect;
    public GameObject[] gates;
    private Collider2D myCollider;

    void Start() {
        myCollider = GetComponent<Collider2D>();
    }

    // Do a check whenever the player enters and exits the keyhole's radius
    public void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            // Check if the player has a key
            PlayerVariables pv = coll.GetComponent<PlayerVariables>();
            if(pv.hasKey) OpenGate(pv);
            else keyIcon.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D coll) {
        if(coll.tag == "Player") {
            keyIcon.SetActive(false);
        }
    }

    public void OpenGate(PlayerVariables player) {
        player.TogglePlayerKey();
        myCollider.enabled = false;
        GameObject endEffect = Instantiate(keyOpenEffect, transform.position, Quaternion.identity);
        Destroy(endEffect, 1.5f);
        foreach(GameObject gate in gates) {
            gate.SetActive(false);
        }
    }
}
