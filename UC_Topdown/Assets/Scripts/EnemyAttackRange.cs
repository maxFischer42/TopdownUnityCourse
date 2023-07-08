using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public EnemyCombat enemyCombat;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player") {
            enemyCombat.isInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.tag == "Player") {
            enemyCombat.isInRange = false;
        }
    }
}
