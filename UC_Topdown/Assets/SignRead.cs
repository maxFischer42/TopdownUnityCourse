using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignRead : MonoBehaviour
{
    public List<string> messages = new List<string>();
    public bool isHolding = false;

    public bool isNear = false;
    public bool isReading = false;

    public GameObject notif;

    public void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            notif.SetActive(true);
            isNear = true;
        }
    }

    public void OnTriggerExit2D(Collider2D coll) {
        if(coll.tag == "Player") {
            notif.SetActive(false);
            isNear = false;
        }
    }


    public void Update() {
        if(isNear && Input.GetButtonDown("Interact") && !isReading) {
            isReading = true;
            InitiateTextbox();
        }
    }

    public void InitiateTextbox() {
        GameManager.Instance.typewriter.StartTextTyping(messages, this.transform);
    }

    public void EndReading() {
        isReading = false;
    }
}
