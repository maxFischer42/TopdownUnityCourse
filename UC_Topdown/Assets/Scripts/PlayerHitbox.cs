using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage;
    public bool hasKnockback = true;
    public float knockbackForce;
    public List<string> tagsToHit;

    public float stunDuration = 0.5f;

    public GameObject attackHitParticles;

    void OnTriggerEnter2D(Collider2D obj) {
        if(tagsToHit.Contains(obj.tag)) {
            if(obj.transform.GetComponent<EnemyHealth>() == false) return;

            Vector2 collisionPoint = obj.transform.position - transform.position;
            Vector2 direction = collisionPoint.normalized;
            EnemyHealth health = obj.GetComponent<EnemyHealth>();
            print("Damage dealt!");
            health.StunObject(stunDuration);
            if(hasKnockback) {
                health.Damage(damage, direction, knockbackForce);
            } else {
                health.Damage(damage);
            }
            SpawnParticles(obj);
        }
    }

    void SpawnParticles(Collider2D obj) {
        Vector2 spawnPosition = obj.transform.position;
        GameObject particles = Instantiate(attackHitParticles, spawnPosition, Quaternion.identity);
        Destroy(particles, 1f);
    }
 
}
