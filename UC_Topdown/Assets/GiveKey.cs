using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveKey : MonoBehaviour
{
    public int keys = 1;

    public void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            //GameManager.Instance.giveKey(keys);
            GameObject.FindObjectOfType<GameManager_>().giveKey(keys);
            Destroy(this.gameObject);
        }
    }
}
