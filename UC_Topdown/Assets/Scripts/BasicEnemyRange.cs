using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyRange : MonoBehaviour
{
    public Transform target;
    public bool targetIsInRange = false;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player") {
            if(collider.GetComponent<PlayerCombat>().isSlime) return;
            target = collider.transform;
            targetIsInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.tag == "Player") {
            target = null;
            targetIsInRange = false;
        }
    }
}
