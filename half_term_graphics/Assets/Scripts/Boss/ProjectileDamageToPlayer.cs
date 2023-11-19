/// <summary>
/// The ProjectileDamageToPlayer class is responsible for handling the collision of projectiles with the player.
/// It applies damage to the player upon impact.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageToPlayer : MonoBehaviour
{
    public int damage = 10;

    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update. It finds and sets up a reference to the GameManager.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// OnTriggerEnter is called when the projectile collides with another collider.
    /// If the collided object is the player, it applies damage and destroys the projectile.
    /// </summary>
    /// <param name="other">The Collider that this projectile has collided with.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.bossDead == false)
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
