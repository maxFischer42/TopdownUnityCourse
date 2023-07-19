using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public bool isOnCooldown = false;
    public float cooldownLength = 0.6f;

    public PlayerAnimator animator;
    public Vector2 direction = new Vector2(1, 0);
    public GameObject hitboxPrefab;
    public float offset = 0.7f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && isOnCooldown == false) {
            animator.DoAttack();
            isOnCooldown = true;
            Invoke("EndCooldown", cooldownLength);
            SpawnHitbox();
        }    

        direction.x = (int)animator.animator.GetFloat("x_velocity");
        direction.y = (int)animator.animator.GetFloat("y_velocity");
    }

    void EndCooldown() {
        isOnCooldown = false;
    }

    void SpawnHitbox() {
        Vector2 spawnOffset = direction * offset;
        GameObject hitbox = Instantiate(hitboxPrefab, transform.position + (Vector3)spawnOffset, Quaternion.identity);
        Destroy(hitbox.gameObject, 0.1f);
    }
}
