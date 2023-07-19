using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public string tagToHurt = "Enemy_Hitbox";

    public Color32 defaultColor;
    public Color32 hurtColor;
    public SpriteRenderer spriteRenderer;

    public bool canGetKnockedBack = true;
    public bool isKnockedBack = false;
    public Vector2 knockbackDirection;
    public float counterForce = 1f;

    public Behaviour combatScript;
    public Behaviour movementScript;
    public GameObject deathEffect;
    public bool hasDied = false;

    void Start() {
        defaultColor = spriteRenderer.color;
        currentHealth = maxHealth;
    }

    void Update() {
        if(currentHealth <= 0 && !hasDied) {
            hasDied = true;
            GameObject drop = Instantiate(deathEffect, transform.position, Quaternion.identity);
            combatScript.enabled = false;
            movementScript.enabled = false;
            spriteRenderer.enabled = false;
            GameObject.FindObjectOfType<GameManager_>().ResetLevel();
            //GameManager.Instance.ResetLevel();
        }
    }

            
    void FixedUpdate(){
        if(isKnockedBack) {
            Vector2 counter = knockbackDirection * counterForce * -1;
            GetComponent<Rigidbody2D>().AddForce(counter);
            if(GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f) {
                isKnockedBack = false;
                combatScript.enabled = true;
                movementScript.enabled = true;
            }
        }
    }

    public void Damage(int damage) {
        currentHealth = currentHealth - damage;
        spriteRenderer.color = hurtColor;
        Invoke("ResetColor", 0.1f);
    }

    public void ApplyKnockback(Vector2 direction) {
        GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        isKnockedBack = true;
        combatScript.enabled = false;
        movementScript.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == tagToHurt) {
            int dmg = coll.GetComponent<HitboxData>().damage;
            Damage(dmg);

            if(canGetKnockedBack) {
                Vector2 direction = (transform.position - coll.transform.position).normalized;
                direction = direction * coll.GetComponent<HitboxData>().knockbackForce;
                knockbackDirection = direction;
                ApplyKnockback(direction);
            }
        }
    }

    void ResetColor() {
        spriteRenderer.color = defaultColor;
    }
}
