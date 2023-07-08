using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationHandler : MonoBehaviour
{
    public PauseManager objToCall;
    public void PauseEndHandle() {
        objToCall.PausingHasEnded();
    }

    public void PauseStartHandle() {
        objToCall.PauseEnded();
    }
}
