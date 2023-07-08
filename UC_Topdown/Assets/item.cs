using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{

    public enum item_type {crystal, gold, heart}
    public item_type type;
    public int amount;

    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {            
            switch (type)
            {  
                case item_type.crystal:
                    GameManager.Instance.score+=amount;
                    GameManager.Instance.PlaySound(GameManager.CollectableTypes.crystal);
                    break;
                case item_type.gold:
                    GameManager.Instance.currency+=amount;
                    GameManager.Instance.PlaySound(GameManager.CollectableTypes.gold);
                    break;
                case item_type.heart:
                    GameManager.Instance.player.GetComponent<PlayerHealthManager>().Heal(amount);
                    GameManager.Instance.PlaySound(GameManager.CollectableTypes.heart);
                    break;
            }
            Destroy(this.gameObject);
        }
    }

}
