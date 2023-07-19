using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script contains information about our current level.

// By default, it requires "Jukebox.cs" to work properly.
public class LevelInfo : MonoBehaviour
{
    // A reference to which song should be playing in this level
    public MusicList levelMusic;

    // A list of all of the positions the player can spawn in the level.
    public Transform[] playerSpawnPositions;

    // A reference to the player gameObject in the level.
    public GameObject playerGameObject;

    void Start() {
        playerGameObject = GameObject.Find("Player");
    }

    // This function will move the player gameObject to the desired spawn point
    public void SpawnPlayerAtLocation(int point) {
        playerGameObject.transform.position = playerSpawnPositions[point].position;
    }

}
