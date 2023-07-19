using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{

    public string levelToLoad = "SampleScene"; 

    public void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            //GameManager.Instance.LoadSceneByName(levelToLoad);
        }
    }
}
