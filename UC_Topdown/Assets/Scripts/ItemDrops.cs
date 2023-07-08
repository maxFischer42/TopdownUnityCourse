using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrops : MonoBehaviour
{
    public GameObject[] common_drops;
    public GameObject[] rare_drops;

    public Vector2Int dropCountRange = new Vector2Int(0, 1);

    [Range(0f, 1f)]
    public float randomChance = 0.1f;

    public void SpawnItems() {
        int dropCount = GetDropCount();     
        bool isRare = isRareDrop();   
        GameObject objectToSpawn;
        for(int i = 0; i < dropCount; i++) {
            if(isRare) {
                objectToSpawn = GetRare();
            } else {
                objectToSpawn = GetCommon();
            }
            Spawn(objectToSpawn, i, dropCount);
        }
    }

    // Get the number of item drops
    int GetDropCount() {
        int rand = Random.Range(dropCountRange.x, dropCountRange.y + 1);
        return rand;
    }

    // Get whether that this item drop will be rare
    bool isRareDrop() {
        float rand = Random.Range(0f, 1f);
        if(rand <= randomChance) {
            return true;
        } else {
            return false;
        }
    }

    GameObject GetRare() {
        return rare_drops[Random.Range(0, rare_drops.Length)];
    }

    GameObject GetCommon() {
        return common_drops[Random.Range(0, common_drops.Length)];
    }

    void Spawn(GameObject item, int index, int length) {    
        Vector3 spawnPos = new Vector3();
        float rt = Mathf.Sqrt(length);
        float radius = item.GetComponent<CircleCollider2D>().radius;
        if(index > rt) {
            spawnPos.y = radius;
            index -= Mathf.RoundToInt(rt);
        }
        spawnPos.x = index * rt / 2f;
        
        Instantiate(item, transform.position + spawnPos, Quaternion.identity);
    }

}
