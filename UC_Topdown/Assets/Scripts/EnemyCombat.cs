using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public GameObject AttackPrefab;
    public float attackCooldown = 3f;
    private bool isOnCooldown;

    // Is the player in range of the enemy to attack?
    public bool isInRange = false;

    public void RangeCheck(bool state) {
        isInRange = state;
    }

    void EndCooldown() {
        isOnCooldown = false;
    }

    void Update() {
        if(isOnCooldown) return;
        if(isInRange) {
            // If the attack is off of cooldown, attack the player in range
            Attack();
        }
    }

    void Attack() {
        GameObject hitbox = Instantiate(AttackPrefab, transform.position, Quaternion.identity);
        Destroy(hitbox, 0.1f);
        isOnCooldown = true;
        Invoke("EndCooldown", attackCooldown);
    }
}
