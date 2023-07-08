using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using UI for making health bars
using UnityEngine.UI;

public class BreakableHealth : MonoBehaviour
{
    // A reference to the spriterenderer
    private SpriteRenderer spriteRenderer;

    // A reference to the effect we want to spawn when the gameobject dies
    public GameObject deathEffect;

    public void OnTriggerEnter2D(Collider2D coll) {

        if(coll.gameObject.tag != "Player_Hurtbox") return;
        // Spawn the item drops for this enemy
        GetComponent<ItemDrops>().SpawnItems();

        // Spawn the death effect for this enemy
        Instantiate(deathEffect, transform.position, transform.rotation);

        // Destroy the gameobject this script belongs to
        Destroy(this.gameObject);
    }

}
