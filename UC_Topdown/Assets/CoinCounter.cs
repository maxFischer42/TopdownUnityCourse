using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public int coins = 0;

    public TextMeshProUGUI textDisplay;

    public void OnTriggerEnter2D(Collider2D coll) {

        if(coll.gameObject.tag == "Coin") {
            coins++;
            Destroy(coll.gameObject);
            textDisplay.text = "Coins: " + coins;
        }
    }

}
