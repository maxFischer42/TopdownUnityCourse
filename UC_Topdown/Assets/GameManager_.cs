using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager_ : MonoBehaviour
{
    private int keyCount = 0;

    void Start() {
        DontDestroyOnLoad(this);
    }

    public int getKeyCount() {
        return keyCount;
    }

    public void useKeys(int numKeys) {
        keyCount = keyCount - numKeys;
    }

    public void giveKey(int numKeys) {
        keyCount = keyCount + numKeys;
    }

    public void ResetLevel() {
        keyCount = 0;

        Invoke("ReloadScene", 3f);
    }

    public void ReloadScene() {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneID);
    }

    public void LoadSceneByName(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

}
