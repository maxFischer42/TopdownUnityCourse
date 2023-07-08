using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private Animator animator; 

    void Start() {
        // For the door object, the rendering component is on the
        // first child object (0).
        animator = transform.GetChild(0).GetComponent<Animator>(); 
    }
    
    public void Open() {
        animator.SetBool("IsOpen", true);
    }

    public void Close() {
        animator.SetBool("IsOpen", false);
    }

}
