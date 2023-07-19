using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGate : MonoBehaviour
{
    public int requiredKeys = 1;

    public void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            //int currentKeys = GameManager.Instance.getKeyCount();
            int currentKeys = GameObject.FindObjectOfType<GameManager_>().getKeyCount();
            if(currentKeys >= requiredKeys) {
                //GameManager.Instance.useKeys(requiredKeys);
                GameObject.FindObjectOfType<GameManager_>().useKeys(requiredKeys);
                Destroy(this.gameObject);
            }
        }
    }
}

            