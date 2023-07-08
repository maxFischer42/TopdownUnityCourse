using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    public bool isActive = false;

    public GameObject wiringOff;
    public GameObject wiringOn;
    public Door door;

    public AudioSource speaker;
    public AudioClip onSound;
    public AudioClip offSound;

    // Handle the events for the pressure plate
    public void SetPressure(bool state) {
        speaker.volume = GameManager.Instance.soundVolume;

        // If state is TRUE, the button is being pushed
        if(state) {
            speaker.PlayOneShot(onSound);
            door.Open(); 
        } 
        // if state is NOT TRUE (false), the button is not being pushed
        else if (!state) {
            speaker.PlayOneShot(offSound);
            door.Close();
        }

        // Turn our wires either on or off based on the state of the pressure plate
        wiringOff.SetActive(!state);
        wiringOn.SetActive(state);
    }    
}
