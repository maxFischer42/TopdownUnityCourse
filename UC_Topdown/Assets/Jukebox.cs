using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is used by our GameManager object.
// It contains references to each song in our game so that we can load it into
// the Jukebox gameObject when the level loads

// Note, this script by default requires the "LevelInfo.cs" script
// as well to work.

public class Jukebox : MonoBehaviour
{  

    // A list of all of our music in the game
    public List<AudioClip> musicList = new List<AudioClip>();

    // This function will return the song that should play in this level
    // as defined in the LevelInfo gameObject
    public AudioClip getLevelSong(LevelInfo info) {
        AudioClip clip = null;
        switch(info.levelMusic) {
            case MusicList.mainMenu:
                clip = musicList[0];
                break;
            case MusicList.level0:
                clip = musicList[1];    
                break;
            case MusicList.level1:
                clip = musicList[2];
                break;
            case MusicList.level2:
                clip = musicList[3];
                break;
            case MusicList.boss:
                clip = musicList[4];
                break;
            case MusicList.menu:
                clip = musicList[5];
                break;
            case MusicList.end:
                clip = musicList[6];
                break;
        }
        return clip;
    }

    // This function will look for an object called "Jukebox" in the scene and begin to play
    // the background music
    public void LoadLevelMusic(AudioClip clip) {
        AudioSource jukebox = GameObject.FindGameObjectWithTag("Jukebox").GetComponent<AudioSource>();
        jukebox.clip = clip;
        jukebox.Play();
    }
}

public enum MusicList {mainMenu, level0, level1, level2, boss, menu, end}