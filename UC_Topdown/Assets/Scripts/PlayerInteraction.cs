using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Whether or not we're currently pushing an object
    public bool isPushing;

    public PlayerMovement playerMovement;

    // If we're pushing, a reference to the object we're pushing
    private Transform pushableRef;

    public void Update() {
        if(isPushing && Input.GetButtonDown("Interact")) {
            pushableRef.GetComponent<Pushable>().HandleGrab();
        }
    }

        // Called from a SendMessage in the Pushable.cs script
    public void TogglePushable(PushableInfo pushable) {
        pushableRef = pushable.transform;
        playerMovement.lockedAxis = pushable.direction;
        isPushing = !isPushing;
    }
}
