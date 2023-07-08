using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private Rigidbody2D rb;

    // The movement component will be disabled while the
    // object is stunned
    public Behaviour movementComponent;

    // A Reference to our Animator component
    private Animator anim;

    // A check whether we will be handling knockback
    private bool isKnockedBack = false;

        // Layermasks
    public LayerMask wallMasks;

    public Sprite FullHeartSprite;
    public Sprite HalfHeartSprite;
    public Sprite EmptyHeartSprite;

    public Transform heartsParent;
    public GameObject healthUIObject;

    private List<GameObject> heartImages = new List<GameObject>();

    // A reference to our combat script
    public PlayerCombat combat;

    void Start() {
        // On startup, set our current health to our max health
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        anim = transform.GetChild(0).GetComponent<Animator>();

        RedrawHealth();

       // spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
       // defaultColor = spriteRenderer.color;
    }

    // Damage the object without any knockback
    public void Damage(int hit) {
        if(combat.isSlime) return;
        ChangeHealth(hit, false);
    }

    // Damage the object and apply knockback
    public void Damage(int hit, Vector2 hitDirection, float hitForce) {
        if(combat.isSlime) return;
        HandleKnockback(hitDirection, hitForce);
        ChangeHealth(hit, false);
    }

    public void Heal(int hp) {
        ChangeHealth(-hp, true);
    }

    void ChangeHealth(int difference, bool isHeal) {
        if(!isHeal) {
            anim.SetBool("isHurt", true);
        }

        // Update the health by applying damage effect;
        currentHealth -= difference;                
        if(currentHealth <= 0) {
            KillObject();
        } else if(difference > 0) {
            PlayHurtSound();
        }

        // Note; if we wanted to be able to heal the object,
        // We could pass a negative number into this function

        // If the current health is not equal to the max health,
        // lets show the health bar
       // if(currentHealth < maxHealth) {
       //     healthbarParent.SetActive(true);
       // } else healthbarParent.SetActive(false);

        // update the healthbar
        //ChangeHealthbar();
        // Don't update the healthbar if the health is over the max health
        if(currentHealth <= maxHealth) RedrawHealth();
    }

    void KillObject() 
    {
        anim.SetBool("isDefeated", true);
        movementComponent.enabled = false;
    }

    void Update() {
        // Make sure the object doesnt over-heal
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        //if(isKnockedBack) HandleKnockback();
    }

    public void StunObject(float duration) {

        // Disable the movement script for this gameobject
        movementComponent.enabled = false;

        // Apply the stun color to the renderer
       // spriteRenderer.color = stunColor;

        // Call the UnStun function after "duration" seconds
        // have passed
        Invoke("UnStun", duration);
    }

    void UnStun() {
        // Restore the default color of the renderer
     //   spriteRenderer.color = defaultColor;
        movementComponent.enabled = true;
        anim.SetBool("isHurt", false);
    }

    private Vector2 currentKnockback;
    private float currentForce;

    void HandleKnockback() {
        // Check if the position is valid or not
        bool isInvalid = Physics2D.OverlapArea(transform.position, transform.position + (Vector3)currentKnockback, wallMasks);
        if(!isInvalid) {
            transform.position = transform.position + (Vector3)currentKnockback;
            currentKnockback = currentKnockback / 2;
            currentForce = currentForce / 2;
            if(currentForce < 0.01f) {
                isKnockedBack = false;
            }
        }
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

    void RedrawHealth() {
        int numHearts = maxHealth / 2;

        // Check to make sure we have the correct number of hearts
        int difference = numHearts - heartImages.Count;
        if(difference < 0) {
            RemoveHeartImage(Mathf.Abs(difference));
        } else if (difference > 0) {
            AddHeartImage(Mathf.Abs(difference));
        }

        
        // Reset all of the hearts to be empty;
        foreach(GameObject obj in heartImages) {
            obj.GetComponentInChildren<Image>().sprite = EmptyHeartSprite;
        }

        int numFullHearts = currentHealth / 2;
        bool doHalfHeart = currentHealth % 2 != 0;
        int i;
        for(i = 0; i < numFullHearts; i++) {
            heartImages[i].GetComponentInChildren<Image>().sprite = FullHeartSprite;
        }
        if(doHalfHeart) {
            heartImages[i].GetComponentInChildren<Image>().sprite = HalfHeartSprite;
        }

        if(currentHealth <= 2) {
            foreach(GameObject obj in heartImages) {
                obj.transform.GetChild(0).GetComponent<HeartAnimation>().lowhealth = true;
            } 
        } else {
            foreach(GameObject obj in heartImages) {
                obj.transform.GetChild(0).GetComponent<HeartAnimation>().lowhealth = false;
            } 
        }
        
    }

    void AddHeartImage(int numHearts) {
        bool isEven = false;
        for(int i = 0; i < numHearts; i++) {
            GameObject newHeart = Instantiate(healthUIObject, heartsParent);
            heartImages.Add(newHeart);
            newHeart.transform.GetChild(0).GetComponent<HeartAnimation>().SetState(isEven);
            isEven = !isEven;
        }
            
    }

    void RemoveHeartImage(int numHearts) {
        for(int i = 0; i < numHearts; i++) {
            GameObject objToRemove = heartImages[heartImages.Count].gameObject;
            heartImages.RemoveAt(heartImages.Count);
            Destroy(objToRemove);
        }
    }

    
    public AudioSource speaker;
    public AudioClip hurtSound;
    public void PlayHurtSound() {
        speaker.volume = GameManager.Instance.soundVolume;
        speaker.PlayOneShot(hurtSound);
    }

}
