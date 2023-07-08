using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float pushSpeed = 3f;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isPushing;
    Vector2 movement;
    float previousDirection = -1;
    public PlayerInteraction interaction;
    public bool disableMovement = false;

    // A unit vector used for multiplying our movement by if
    // we want to only move in one axis
    public Vector2 lockedAxis;

    void Update() {
        if(disableMovement) {
            movement.x = 0f;
            movement.y = 0f;
        } else {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        if(movement.x != 0 || movement.y != 0) {
            animator.SetFloat("Speed", 1f);
        } else {
            animator.SetFloat("Speed", 0f);
        } 

        if(movement.x != 0 && movement.x != previousDirection) {
            FlipSprite();
        }
    }

    void FixedUpdate() {
        if(animator.GetBool("Attack") == true || disableMovement) {
            rb.velocity = Vector2.zero;
        } else {
            float movementMultiplier = moveSpeed;
            if(interaction.isPushing) {
                movementMultiplier = pushSpeed;
                movement *= lockedAxis;
            }
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void FlipSprite() {
        if(previousDirection == 1) {
            spriteRenderer.flipX = false;
        } else if (previousDirection == -1) {
            spriteRenderer.flipX = true;
        }
        previousDirection = movement.x;
    }
}
