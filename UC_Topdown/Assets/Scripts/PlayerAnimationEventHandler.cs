using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    // A reference to the player movement script
    public PlayerCombat combatScript;

    public enum playerEventType {primaryAttack, specialAttack, sneakAttack}

    public void HandleEvent(playerEventType eventToDo) {
        switch(eventToDo) {
            case playerEventType.primaryAttack:
                combatScript.SpawnPrimaryHitbox();
                break;
            case playerEventType.specialAttack:
                combatScript.SpawnSpecialHitbox();
                break;
            case playerEventType.sneakAttack:
                combatScript.SpawnSneakHitbox();
                break;
        }
    }
}
