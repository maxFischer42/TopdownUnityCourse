using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimation : MonoBehaviour
{
    public bool lowhealth;
    public Animator anim;

    public void SetState(bool even) {
        anim.SetBool("isEven", even);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isLow", lowhealth);
    }
}
