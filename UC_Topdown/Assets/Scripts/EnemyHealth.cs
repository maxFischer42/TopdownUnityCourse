using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using UI for making health bars
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private Rigidbody2D rb;

    public GameObject healthbarParent;
    public Image healthbar;

    // A reference to the spriterenderer
    private SpriteRenderer spriteRenderer;

    // The default color of this sprite
    private Color32 defaultColor;

    // The color we want for the stun effect
    public Color32 stunColor;

    // The movement component will be disabled while the
    // object is stunned
    public Behaviour movementComponent;

    // A check whether we will be handling knockback
    private bool isKnockedBack = false;

    // Layermasks
    public LayerMask wallMasks;

    // A reference to the effect we want to spawn when the gameobject dies
    public GameObject deathEffect;

    void Start() {
        // On startup, set our current health to our max health
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    // Damage the object without any knockback
    public void Damage(int hit) {
        ChangeHealth(hit);
    }

    // Damage the object and apply knockback
    public void Damage(int hit, Vector2 hitDirection, float hitForce) {
        HandleKnockback(hitDirection, hitForce);
        ChangeHealth(hit);
    }


    // Knocks the object back based on a direction and force
    // using the Rigidbody AddForce function
    void HandleKnockback(Vector2 direction, float force) {
        // direction is a unit vector telling us the direction the
        // object was hit from. The inverse of it should be 
        // the direction we want to apply the knockback to

        Vector2 inverseDirection = direction;

        Vector2 knockbackForce = inverseDirection * force;

        isKnockedBack = true;
        currentKnockback = knockbackForce;
        currentForce = force;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    void ChangeHealth(int difference) {
        // Update the health by applying damage effect;
        currentHealth -= difference;                
        if(currentHealth <= 0) {
            KillObject();
        } else if(difference > 0) {
            PlayHurtSound();
        }
        movementComponent.enabled = false;
        // Note; if we wanted to be able to heal the object,
        // We could pass a negative number into this function

        // If the current health is not equal to the max health,
        // lets show the health bar
        if(currentHealth < maxHealth) {
            healthbarParent.SetActive(true);
        } else healthbarParent.SetActive(false);

        // update the healthbar
        ChangeHealthbar();
    }

    // Update the graphic of the healthbar.
    void ChangeHealthbar() {
        // We will be using the Unity UI Image component for
        // The health bar. 

        // Note, when setting the anchor for the Image component in the editor,
        // Hold down shift to make blue dots appear
        // these dots will help anchor the element to a given side

        // The healthbar's value will be determined by the ratio of
        // current health to full health
        // We have to cast currenthealth and maxhealth to floats
        // since dividing ints will round to an int

        float healthRatio = (float)currentHealth / (float)maxHealth;
        float yVal = 0.1f;
        healthbar.rectTransform.sizeDelta = new Vector2(healthRatio, yVal);
    }

    void KillObject() 
    {  
        // Spawn the item drops for this enemy
        GetComponent<ItemDrops>().SpawnItems();

        // Spawn the death effect for this enemy
        Instantiate(deathEffect, transform.position, transform.rotation);

        // Destroy the gameobject this script belongs to
        Destroy(this.gameObject);
    }

    void Update() {
        // Make sure the object doesnt over-heal
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        if(isKnockedBack) {
            if(rb.velocity.magnitude < 0.1f) {
                movementComponent.enabled = true;
                isKnockedBack = false;
            }
        }
    }

    public void StunObject(float duration) {

        // Disable the movement script for this gameobject
        movementComponent.enabled = false;

        // Apply the stun color to the renderer
        spriteRenderer.color = stunColor;

        // Call the UnStun function after "duration" seconds
        // have passed
        Invoke("UnStun", duration);
    }

    void UnStun() {
        // Restore the default color of the renderer
        spriteRenderer.color = defaultColor;
      //  movementComponent.enabled = true;
    }

    private Vector2 currentKnockback;
    private float currentForce;

    public AudioSource speaker;
    public AudioClip hurtSound;
    public void PlayHurtSound() {
        speaker.volume = GameManager.Instance.soundVolume;
        speaker.PlayOneShot(hurtSound);
    }

}
