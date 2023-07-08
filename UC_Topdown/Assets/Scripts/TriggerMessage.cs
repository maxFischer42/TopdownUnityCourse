using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour
{

    // Our reciever object
    public Transform reciever;

    // What is the function name for our reciever function
    public string message = "OnTrigger";

    // Tag we are looking for in this trigger
    public string tagName;

    // An optional direction tag used for pushable objects
    public Vector2 direction; 

    // Detects when a rigidbody enters this object's hitbox
    void OnTriggerEnter2D(Collider2D objectCollider) {
        if(objectCollider.tag == tagName) { 
            reciever.SendMessage(message, new TriggerInformation(objectCollider, TriggerState.Enter, direction));            
        }
    } 

    // Detects when a rigidbody stays within this object's hitbox
    void OnTriggerStay2D(Collider2D objectCollider) {
        if(objectCollider.tag == tagName) { 
            reciever.SendMessage(message, new TriggerInformation(objectCollider, TriggerState.Stay, direction));     
        }
    } 

    // Detects when a rigidbody leaves this object's hitbox
    void OnTriggerExit2D(Collider2D objectCollider) {
            if(objectCollider.tag == tagName) { 
                reciever.SendMessage(message, new TriggerInformation(objectCollider, TriggerState.Exit, direction));            
            }
        } 

}

// This class denotes the contents of the message we will be sending
public class TriggerInformation {
    public Collider2D objectCollider;
    public TriggerState state;
    public Vector2 side;

    // Constructors; called when this class is created
    public TriggerInformation(Collider2D obj, TriggerState mode, Vector2 dir) {
        objectCollider = obj;
        state = mode;
        side = dir;
    }
}

// An enumerator used to dictate in our TriggerInformation message whether
// the trigger method is Enter, Stay, or Exit.
public enum TriggerState {Enter, Stay, Exit}