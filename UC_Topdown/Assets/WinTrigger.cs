using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider) {
        if(collider.name == "Player") {            
            collider.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
