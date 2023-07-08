using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMovement : MonoBehaviour
{
    // How fast we should move
    public float moveSpeed;

    // How fast should we move when pushing/pulling something?
    public float pushSpeed;

    // A reference to the Rigidbody2D component on our player
    private Rigidbody2D rb;

    // A reference to the Animator component on our player
    public Animator playerAnimator;

    // A reference to the Sprite Renderer component for our player
    private SpriteRenderer spriteRenderer;

    // What X direction did we face last time we flipped?
    private int previousDirection = 0;

    // Is our character in slime form? 
    public bool isSlime = false;

    // Is our character under a cooldown right now?
    public bool isOnCooldown = false;

    public float SlimeCooldown = 0.4f;
    public float AttackCooldown = 0.2f;
    public float SpecialCooldown = 0.35f;

    public float timeToCastSpecial = 0.1f;

    public GameObject SpecialAttackPrefab;

    public bool isPushing = false;

    // A unit vector used for multiplying our movement by if
    // we want to only move in one axis
    private Vector2 lockedAxis;

    // If we're pushing, a reference to the object we're pushing
    private Transform pushableRef;

    // An attack prefabs class to store all of our attack information
    public AttackPrefabs attackPrefabs;

    // A reference to the cooldown manager script to handle our attack cooldowns
    public CooldownManager cooldownManager;

    // A reference to the pixel count our graphics use
    private int subPixelMeasurements = 16;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = playerAnimator.GetComponent<SpriteRenderer>();
        previousDirection = (spriteRenderer.flipX ? 1 : -1);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPushing == false) {
            if(isOnCooldown) {
                rb.velocity = Vector2.zero;
                return;
            }
            if(Input.GetButtonDown("Jump") && !isSlime && !cooldownManager.GetCooldownStatus(2)) {
                isSlime = true;
                playerAnimator.SetBool("IsSlime", true);
                cooldownManager.BeginCooldown(2);
                return;
            } else if(Input.GetButtonUp("Jump") && isSlime) {
                isSlime = false;
                playerAnimator.SetBool("IsSlime", false);
                playerAnimator.SetTrigger("Regrow");
                isOnCooldown = true;
                Invoke("EndCooldown", SlimeCooldown);
                return;
            }

            if(Input.GetButtonDown("Fire1") && !isSlime&& !cooldownManager.GetCooldownStatus(0)) {
                playerAnimator.SetTrigger("Attack");
                isOnCooldown = true;
                cooldownManager.BeginCooldown(0);
                Invoke("EndCooldown", AttackCooldown);
                return;
            }

            if(Input.GetButtonDown("Fire2") && !isSlime&& !cooldownManager.GetCooldownStatus(1)) {
                playerAnimator.SetTrigger("Special");
                isOnCooldown = true;
                cooldownManager.BeginCooldown(1);
                Invoke("SpawnSpecialAttack", timeToCastSpecial);
                return;
            }
        }

        if(isPushing && Input.GetButtonDown("Interact")) {
            pushableRef.GetComponent<Pushable>().HandleGrab();
        }
        
        if(!isSlime) {

            Vector2 velocity;
            float speed = isPushing ? pushSpeed : moveSpeed;
            velocity.x = Mathf.Round((Input.GetAxisRaw("Horizontal") * speed) / subPixelMeasurements) / subPixelMeasurements;
            velocity.y = Mathf.Round((Input.GetAxisRaw("Vertical") * speed) / subPixelMeasurements) / subPixelMeasurements;
            if(isPushing) velocity = velocity * lockedAxis;
            if(velocity.x != 0 && Mathf.Sign(velocity.x) != previousDirection && !isPushing) {
                previousDirection = (int)Mathf.Sign(velocity.x);
                spriteRenderer.flipX = (Mathf.Sign(velocity.x) == 1 ? true : false);
            }

            rb.velocity = velocity;
            playerAnimator.SetFloat("Speed", Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y));
        } else {
            rb.velocity = Vector2.zero;
        }
    }

    void EndCooldown() {
        isOnCooldown = false;
    }

    void SpawnSpecialAttack() {
        GameObject specialAttack = Instantiate(SpecialAttackPrefab, transform.position + SpecialAttackPrefab.transform.position, Quaternion.identity);
        Destroy(specialAttack, 1f);



        Invoke("EndCooldown", SpecialCooldown);
    }

    // Called from a SendMessage in the Pushable.cs script
    public void TogglePushable(PushableInfo pushable) {
        pushableRef = pushable.transform;
        lockedAxis = pushable.direction;
        isPushing = !isPushing;
    }

    // The spawn hitbox prefabs are called via animation events instead
    // of inside of the script

    public void SpawnPrimaryHitbox() {
        GameObject primaryAttackHitbox = Instantiate(attackPrefabs.primaryAttackPrefab, (Vector2)transform.position + attackPrefabs.primaryOffset, Quaternion.identity);
        Destroy(primaryAttackHitbox, 0.2f);
    }

    public void SpawnSpecialHitbox() {
        GameObject specialAttackHitbox = Instantiate(attackPrefabs.specialAttackPrefab, (Vector2)transform.position + attackPrefabs.specialOffset, Quaternion.identity);
        Destroy(specialAttackHitbox, 0.2f);
    }

    public void SpawnSneakHitbox() {
        GameObject sneakAttackHitbox = Instantiate(attackPrefabs.sneakAttackPrefab, (Vector2)transform.position + attackPrefabs.sneakOffset, Quaternion.identity);
        Destroy(sneakAttackHitbox, 0.2f);
    }


}

// Add system.serializable tag so we can edit this data in the inspector
[System.Serializable]
public class AttackPrefabs {
    public GameObject primaryAttackPrefab;
    public GameObject specialAttackPrefab;
    public GameObject sneakAttackPrefab;

    public Vector2 primaryOffset = Vector2.zero;
    public Vector2 specialOffset = Vector2.zero;
    public Vector2 sneakOffset = Vector2.zero;
}

