using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    // The side the player grabbed from
    private Vector2 direction = Vector2.zero;   

    // A reference to the gameobject used to show the player can interact with the pushable
    public GameObject notification; 

    // Checks whether or not the player can currently push this pushable
    private bool canPush = false;

    // A reference to the character attempting to push the pushable
    private Collider2D characterToPush;

    // If we're on cooldown, we don't want to grab the pushable
    private bool onCooldown = false;

    // A reference to our rigidbody component
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(onCooldown) return;
        if(canPush && Input.GetButtonUp("Interact")) {
            HandleGrab();
            HandlePopup();
            canPush = false;
        }
    }

    // Our Listener function; is called via a function elsewhere via the SendMessage function
    public void TriggerReciever(TriggerInformation data) {
        switch(data.state) {
            case TriggerState.Enter:
                characterToPush = data.objectCollider;
                direction = data.side;
                if(characterToPush.GetComponent<PlayerInteraction>().isPushing) break;
                canPush = true;
                HandlePopup();
                break;
            case TriggerState.Exit:
                canPush = false;
                HandlePopup();
                break;
        }
    }

    void HandlePopup() {
        if(canPush) notification.SetActive(true);
        else notification.SetActive(false); 
    }

    public void HandleGrab() {
        if(canPush) {
            transform.parent = characterToPush.transform;
            characterToPush.transform.SendMessage("TogglePushable", new PushableInfo(transform, direction));
        } else {
            transform.parent = null;
            characterToPush.transform.SendMessage("TogglePushable", new PushableInfo(transform, direction));
            onCooldown = true;
            Invoke("EndCooldown", 1f);
        }
    }

    void EndCooldown() {
        onCooldown = false;
    }
}

public class PushableInfo {
    public Transform transform;
    public Vector2 direction;
    public PushableInfo(Transform t, Vector2 d) {
        transform = t;
        direction = d;
    }
}