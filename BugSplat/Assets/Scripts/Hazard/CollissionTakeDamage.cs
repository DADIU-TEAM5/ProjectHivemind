using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionTakeDamage : MonoBehaviour
{
    public float PlayerDamage, EnemyDamage;

    public GameEvent OnDamageEvent;

    public bool PlayerTakeDamage(GameObject gameObject) {
        var player = gameObject.GetComponent<PlayerHealth>();
            
        if (player != null) {
            Debug.Log("player take damage");
            player.TakeDamage(PlayerDamage);
            return true;
        }

        return false;
    }

    public bool EnemyTakeDamage(GameObject gameObject) {
        var potentialEnemy = gameObject.GetComponent<Enemy>();

        if (potentialEnemy != null) {
            Debug.Log("enemy take damage");
            potentialEnemy.TakeDamage(EnemyDamage);
            return true;
        }

        return false;
    }
    
    void OnTriggerEnter(Collider collision) {
        if (collision == null) return;

        if (!EnemyTakeDamage(collision.gameObject)) {
            PlayerTakeDamage(collision.gameObject);
        }
    }
}
