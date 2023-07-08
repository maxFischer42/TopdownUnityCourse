using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PauseManager pauseManager;

    public bool isLevelLoaded = false;

    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public int currency = 0;
    public int score = 0;

    public PlayerMovement player;

    private static GameManager _instance;
    private LevelInfo currentLevelInfo;
    public Jukebox jukebox;
    public AudioSource playerSpeaker;
    public AudioClip crystalSound;
    public AudioClip goldSound;
    public AudioClip heartSound;
    public enum CollectableTypes {crystal, gold, heart}

    public static GameManager Instance { 
        get {
            if(_instance == null) {
                Debug.LogError("No game manager instance found!");
            }
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
        if(PlayerPrefs.HasKey("volume_music")) musicVolume = PlayerPrefs.GetFloat("volume_music");
        if(PlayerPrefs.HasKey("volume_sound")) soundVolume = PlayerPrefs.GetFloat("volume_sound");
    }

    void Start() {
        DontDestroyOnLoad(this);
        if(GetComponent<PauseManager>()) pauseManager = GetComponent<PauseManager>();
        OnLevelLoaded(0);
    }

    public void OnLevelExit(int spawnTo, string sceneToLoad) {
        //ScreenFadeOut();
        int currentSceneID = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneToLoad);
        while(SceneManager.GetActiveScene().buildIndex != currentSceneID) {}
        OnLevelLoaded(spawnTo);
    }

    void OnLevelLoaded(int spawnPos) {
        pauseManager = GameObject.FindObjectOfType<PauseManager>();
        pauseManager.OnLevelLoaded();
        pauseManager.soundSlider.value = soundVolume;
        pauseManager.musicSlider.value = musicVolume;
        player = GameObject.FindObjectOfType<PlayerMovement>();
        currentLevelInfo = GameObject.FindObjectOfType<LevelInfo>();
        currentLevelInfo.SpawnPlayerAtLocation(spawnPos);
        //ScreenFadeIn();

        playerSpeaker = player.transform.Find("Soundbox").GetComponent<AudioSource>();

        // Start level music
        jukebox.LoadLevelMusic(jukebox.getLevelSong(currentLevelInfo));
    }

    public void UpdateSound(float value) {
        soundVolume = value;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Soundbox")) {
            obj.GetComponent<AudioSource>().volume = soundVolume;
            PlayerPrefs.SetFloat("volume_sound", value);
        }
    }

    public void UpdateMusic(float value) {
        musicVolume = value;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Jukebox")) {
            obj.GetComponent<AudioSource>().volume = musicVolume;
            PlayerPrefs.SetFloat("volume_music", value);
        }
    }

    public void Update() {
        if(Input.GetButtonDown("Pause")) {
            Debug.Log("Escape pressed");
            if(!pauseManager.pausing) {
                if(pauseManager.isPaused) pauseManager.UnPause();
                if(!pauseManager.isPaused) pauseManager.Pause();
            }
        }
    }

    public void PlaySound(CollectableTypes type) {
        switch(type) {
            case CollectableTypes.crystal:
                playerSpeaker.PlayOneShot(crystalSound);
                break;
            case CollectableTypes.gold:
                playerSpeaker.PlayOneShot(goldSound);
                break;
            case CollectableTypes.heart:
                playerSpeaker.PlayOneShot(heartSound);
                break;
        }
    }
}
