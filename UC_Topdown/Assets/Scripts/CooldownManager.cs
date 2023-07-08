using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public List<AttackCooldown> AttackCooldownObjects;
    public PlayerCombat playerCombat;

    // The default color of the attack image
    public Color ActiveColor = Color.white;

    // The color of the image when the cooldown is active
    public Color InactiveColor = Color.red;

    void Start() {
        UpdateUI();
    }

    void Update() {
        for(int i = 0; i < AttackCooldownObjects.Count; i++) {
            if(AttackCooldownObjects[i].OnCooldown) UpdateCooldown(i);
        }
    }

    public void BeginCooldown(int id) {
        AttackCooldownObjects[id].OnCooldown = true;
        AttackCooldownObjects[id].currentCooldown = 0f;      
        AttackCooldownObjects[id].ScrollCooldownImage.enabled = true;
        AttackCooldownObjects[id].SetColor(InactiveColor);
    }

    public void UpdateCooldown(int id) {
        if(AttackCooldownObjects[id].currentCooldown >= AttackCooldownObjects[id].cooldownLengthInSeconds) {
            EndCooldown(id);
            return;
        }
        AttackCooldownObjects[id].currentCooldown += Time.deltaTime;
        AttackCooldownObjects[id].ScrollCooldownImage.rectTransform.sizeDelta
         = new Vector2((AttackCooldownObjects[id].GetCooldownProgression() * 100) / AttackCooldownObjects[id].cooldownLengthInSeconds, 100);
    }

    public void EndCooldown(int id) {
        AttackCooldownObjects[id].OnCooldown = false;
        AttackCooldownObjects[id].ScrollCooldownImage.enabled = false;
        AttackCooldownObjects[id].SetColor(ActiveColor);
    }

    public bool GetCooldownStatus(int id) {
        return AttackCooldownObjects[id].OnCooldown;
    }

    public void UpdateUI() {
        AttackCooldownObjects[0].parentObject.gameObject.SetActive(playerCombat.canPrimary);
        AttackCooldownObjects[1].parentObject.gameObject.SetActive(playerCombat.canSpecial);
        AttackCooldownObjects[2].parentObject.gameObject.SetActive(playerCombat.canSneak);
    }
}

// Make this class serializable so we can edit it in the inspector
[System.Serializable]
public class AttackCooldown {
    public Transform parentObject;
    public Image ScrollCooldownImage;
    public Image PrimaryCooldownImage;
    public bool OnCooldown = false;
    public float cooldownLengthInSeconds = 2;
    public float currentCooldown;

    public float GetCooldownProgression() {
        //Debug.Log((float)cooldownLengthInSeconds - (float)currentCooldown);
        return (float)cooldownLengthInSeconds - (float)currentCooldown;
    }

    public void SetColor(Color newColor) {
        PrimaryCooldownImage.color = newColor;
    }
 
}