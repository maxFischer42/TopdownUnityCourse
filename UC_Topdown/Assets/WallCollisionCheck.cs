using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallCollisionCheck : MonoBehaviour
{
    // This function is run any time the BoxCollider2D
    // on this object collides with another collider
    public void OnCollisionEnter2D(Collision2D collider) {
        Debug.Log(transform.name + " has hit " + collider.transform.name);
        ReloadScene();
    }
    void ReloadScene() {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneID);
    }
}
