using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public int pointToLoad;
    public string sceneToLoad;

    void OnTriggerEnter2D(Collider2D col) {
        if(col.name == "Player") {
            GameManager.Instance.OnLevelExit(pointToLoad, sceneToLoad);
        }
    }
}
