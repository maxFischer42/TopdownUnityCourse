using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces this gameobject to have a BoxCollider2D component
[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : MonoBehaviour
{
    // A private reference to our Pressure Plate Handler script, on the parent object
    private PressurePlateHandler parentHandler;

    // A private reference to our animator component 
    private Animator animator;

    // A string List that we will use to filter what tags we want to interact with
    // this pressure plate
    public List<string> tagFilter;

    // Is the pressure plate currently being activated?
    public bool isActive = false;

    // A reference to the object currently on the pressure plate
    public Transform weightReference;


    void Start() {
        // Set the parent handler component to the PressurePlateHandler component on our parent object
        parentHandler = transform.parent.GetComponent<PressurePlateHandler>();

        // Set our animator component to the one on our gameobject
        animator = GetComponent<Animator>();

        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D coll) {

        // Check if the object colliding's tag is within our tag list.
        // If it is, and the button is NOT being pressed, tell our
        // handler that it is being pressed.
        if(!isActive && IsTagValid(coll.tag)) {
            isActive = true;
            animator.SetBool("isActive", true);
            parentHandler.SetPressure(isActive);
            weightReference = coll.transform;
        }
    }

    void OnTriggerStay2D(Collider2D coll) {

        // Check if the object colliding's tag is within our tag list.
        // If it is, and the button is NOT being pressed, tell our
        // handler that it is being pressed.
        if(!isActive && IsTagValid(coll.tag)) {
            isActive = true;
            animator.SetBool("isActive", true);
            parentHandler.SetPressure(isActive);
            weightReference = coll.transform;
        }
    }

    void OnTriggerExit2D(Collider2D coll) {
        // Check if the object colliding's tag is within our tag list
        // If it is, and the button is being pressed, and the collider is the same
        // as the one that pushed the button, tell our
        // handler that it is NOT being pressed.
        if(isActive && IsTagValid(coll.tag)) {
            isActive = false;
            animator.SetBool("isActive", false);
            parentHandler.SetPressure(isActive);
        }
    }

    // Checks whether the given tag is valid
    bool IsTagValid(string check) {

        // Loop through every tag we have listed in tagFilter
        // If we find a match, we return TRUE
        // If we don't find a match, we return FALSE
        foreach(string tag in tagFilter) {
            if(check == tag) return true;
        }
        return false;
    }
}
