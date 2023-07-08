using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    public int Damage;
    public float Knockback;
    public float stunDuration = 0.5f;
     public GameObject attackHitParticles;

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.tag == "Player") {
            if(collider.GetComponent<PlayerCombat>().isSlime) return;
            Vector2 collisionPoint = collider.transform.position - transform.position;
            Vector2 direction = collisionPoint.normalized;
            PlayerHealthManager health = collider.GetComponent<PlayerHealthManager>();
            print("Enemy Damage dealt!");
            health.Damage(Damage, direction, Knockback);
            health.StunObject(stunDuration);
            SpawnParticles(collider);
        }
    }

    void SpawnParticles(Collider2D obj) {
        Vector2 spawnPosition = obj.transform.position;
        GameObject particles = Instantiate(attackHitParticles, spawnPosition, Quaternion.identity);
        Destroy(particles, 1f);
    }
}
