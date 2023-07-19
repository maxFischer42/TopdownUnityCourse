using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator_ : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float previousDirection = 1;
    public bool isMoving = false;

    void Update() {
        float x_velocity = Input.GetAxisRaw("Horizontal");
        float y_velocity = Input.GetAxisRaw("Vertical");

        if(x_velocity == 0 && y_velocity == 0) 
        {
            isMoving = false;
        }
        else 
        {
            isMoving = true;
        }

        animator.SetBool("isMoving", isMoving); 

        float currentDirection = Mathf.Sign(x_velocity);
        if(currentDirection != previousDirection && x_velocity != 0) {
            previousDirection = currentDirection;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if(x_velocity != 0 || y_velocity != 0) {
            animator.SetFloat("x_velocity", x_velocity);
            animator.SetFloat("y_velocity", y_velocity);
        }

    }

    // This will be called from a combat script
    public void DoAttack() {
        animator.SetTrigger("attack");
    }
}
