using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public bool hasKey = false;
    public GameObject keyIcon;

    public void TogglePlayerKey() {
        hasKey = !hasKey;
        keyIcon.SetActive(hasKey);
    }
}
