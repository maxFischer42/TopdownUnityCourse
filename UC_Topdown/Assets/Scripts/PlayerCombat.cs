using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    
    public GameObject primaryAttackPrefab;
    public GameObject specialAttackPrefab;
    public GameObject sneakAttackPrefab;

    public Vector2 primaryOffset = Vector2.zero;
    public Vector2 specialOffset = Vector2.zero;
    public Vector2 sneakOffset = Vector2.zero;

    public bool canPrimary = false;
    public bool canSpecial = false;
    public bool canSneak = false;

    public PlayerInteraction interaction;

    public bool isAttacking;
    public bool isSlime;
    public CooldownManager cooldownManager;
    public Animator playerAnimator;

    public float SlimeCooldown;
    public float AttackCooldown;
    public float SpecialCooldown;
    public float timeToCastSpecial;

    public GameObject specialAttackEffect;
    
    public BoxCollider2D playerHitbox;

    public PlayerMovement movement;

    public AudioClip primaryAttackSound;
    public AudioClip specialAttackSound;
    public AudioClip sneakAttackSound;

    public AudioSource speaker;
    

    void Update() {
        if(interaction.isPushing == false) {
            
            if(isAttacking) {
                return;
            }
            
            if(Input.GetKeyDown(KeyCode.Space) && !isSlime && !cooldownManager.GetCooldownStatus(2) && canSneak) {
                movement.disableMovement = true;
                isSlime = true;
                playerAnimator.SetBool("IsSlime", true);
                cooldownManager.BeginCooldown(2);
                playerHitbox.isTrigger = true;
                return;
            } else if(Input.GetKeyUp(KeyCode.Space) && isSlime) {
                movement.disableMovement = false;
                isSlime = false;
                playerAnimator.SetBool("IsSlime", false);
                playerAnimator.SetTrigger("Regrow");
                isAttacking = true;
                Invoke("EndCooldown", SlimeCooldown);
                return;
            }

            if(Input.GetButtonDown("Fire1") && !isSlime && !cooldownManager.GetCooldownStatus(0) && canPrimary) {
                playerAnimator.SetTrigger("Attack");
                isAttacking = true;
                cooldownManager.BeginCooldown(0);
                Invoke("EndCooldown", AttackCooldown);
                return;
            }

            if(Input.GetButtonDown("Fire2") && !isSlime&& !cooldownManager.GetCooldownStatus(1) && canSpecial) {
                playerAnimator.SetTrigger("Special");
                isAttacking = true;
                cooldownManager.BeginCooldown(1);
                Invoke("SpawnSpecialAttack", timeToCastSpecial);
                return;
            }
        }

    }

    
    void EndCooldown() {
        isAttacking = false;
    }


    // The spawn hitbox prefabs are called via animation events instead
    // of inside of the script

    public void SpawnPrimaryHitbox() {
        GameObject primaryAttackHitbox = Instantiate(primaryAttackPrefab, (Vector2)transform.position + primaryOffset, Quaternion.identity);
        Destroy(primaryAttackHitbox, 0.2f);
        PlaySound(primaryAttackSound);
    }

    public void SpawnSpecialHitbox() {
        GameObject specialAttackHitbox = Instantiate(specialAttackPrefab, (Vector2)transform.position + specialOffset, Quaternion.identity);
        Destroy(specialAttackHitbox, 0.2f);
    }

    public void SpawnSneakHitbox() {
        GameObject sneakAttackHitbox = Instantiate(sneakAttackPrefab, (Vector2)transform.position + sneakOffset, Quaternion.identity);
        playerHitbox.isTrigger = false;
        Destroy(sneakAttackHitbox, 0.2f);
        PlaySound(sneakAttackSound);
    }

    void SpawnSpecialAttack() {
        GameObject specialAttack = Instantiate(specialAttackEffect, transform.position + specialAttackEffect.transform.position, Quaternion.identity);
        Destroy(specialAttack, 1f);
        Invoke("EndCooldown", SpecialCooldown);
        PlaySound(specialAttackSound);
    }

    public void PlaySound(AudioClip clip) {
        speaker.PlayOneShot(clip);
    }



}
