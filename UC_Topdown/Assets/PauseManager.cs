using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Using TMPro for text handling
using TMPro;

public class PauseManager : MonoBehaviour
{
    public bool pausing = false;
    public bool isPaused = false;
    public Animator[] animators;
    public Canvas canvas;

    public float timeToPause = 0.25f;
    public float timeToUnpause = 0.25f;

    public Slider soundSlider;
    public Slider musicSlider;

    public AudioClip UiHoverSound;
    public AudioClip UiPressSound;


    // In old Unity, we'd just use "Text" instead of "TextMeshProUGUI"
    public TextMeshProUGUI crystalCount;
    public TextMeshProUGUI goldCount;

    public List<Behaviour> behavioursToDisable = new List<Behaviour>();

    public void OnLevelLoaded() {
        behavioursToDisable.Clear();
        UpdateBehaviors();        
    }

    public void UpdateBehaviors() {
        behavioursToDisable.Add(GameObject.Find("Player").GetComponent<PlayerMovement>());
        behavioursToDisable.Add(GameObject.Find("Player").GetComponent<PlayerCombat>());
        behavioursToDisable.Add(GameObject.Find("Player").GetComponent<CooldownManager>());
        behavioursToDisable.Add(GameObject.Find("Player").GetComponent<PlayerHealthManager>());
        behavioursToDisable.Add(GameObject.Find("Player").GetComponent<PlayerInteraction>());
    }

    void HandleAnimations(bool state) {
        foreach(Animator anim in animators) {
            anim.SetBool("pause", state);
        }
    }

    public void Pause() {
        crystalCount.text = GameManager.Instance.score.ToString();
        goldCount.text = GameManager.Instance.currency.ToString();
        pausing = true;
        Time.timeScale = 0;
        canvas.enabled = true;
        HandleAnimations(true);
        foreach(Behaviour be in behavioursToDisable) {
            be.enabled = false;
        }
        
    }

    public void UnPause() {
        HandleAnimations(false);
    }

    public void PauseEnded() {
        pausing = false;
        isPaused = true;
    }

    public void PausingHasEnded() {
        canvas.enabled = false;
        isPaused = false;
        Time.timeScale = 1f;
        foreach(Behaviour be in behavioursToDisable) {
            be.enabled = true;
        }
    }

    public void ChangeSoundVolume() {
        GameManager.Instance.UpdateSound(soundSlider.value);
    }

    public void ChangeMusicVolume() {
        GameManager.Instance.UpdateMusic(musicSlider.value);
    }

    public void OnHover() {
        GameManager.Instance.playerSpeaker.PlayOneShot(UiHoverSound);
    }

    public void OnPress() {
        GameManager.Instance.playerSpeaker.PlayOneShot(UiPressSound);
    }
}
