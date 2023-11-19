/// <summary>
/// The BossHealth class manages the health of the boss character in the game.
/// It handles damage taken by the boss and updates the health bar UI.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int health = 200;
    private Slider healthBar;
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update. It initializes the boss's health bar
    /// and sets up the reference to the GameManager.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        healthBar = GameObject.FindGameObjectWithTag("BossHealthBar").GetComponent<Slider>();
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    /// <summary>
    /// Applies damage to the boss's health and updates the health bar. If the health drops to 0,
    /// it triggers the boss's death.
    /// </summary>
    /// <param name="damage">The amount of damage to apply to the boss.</param>
    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        if (healthBar != null)
        {
            healthBar.value = health;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles the death of the boss. Notifies the GameManager that the boss is dead
    /// and destroys the boss game object.
    /// </summary>
    void Die()
    {
        gameManager.bossDead = true;
        gameManager.BossDefeated();
        Destroy(gameObject);
    }
}
