using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used on an object that is spawned when another object is destroyed.
// Normally it will play a sound as well as a sort of death animation for the object
// that had spawned it.

public class DeathEffect : MonoBehaviour
{
    // A reference to the Audio Source on this object
    public AudioSource speaker;

    // A reference to the sound that will play when this object is spawned
    public AudioClip deathSound;

    // Do we destroy this object after it has spawned?
    public bool doLeaveRemains = false;

    // If we do destroy this object, how many seconds until we destroy it?
    public float timeToRemove = 2f;

    public void Start() {
        speaker.volume = GameManager.Instance.soundVolume;
        speaker.PlayOneShot(deathSound);
        if(!doLeaveRemains) Destroy(this.gameObject, timeToRemove);
    }
}
