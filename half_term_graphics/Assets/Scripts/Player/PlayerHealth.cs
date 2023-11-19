/// <summary>
/// The PlayerHealth class is responsible for managing the player's health.
/// It handles damage taken by the player and updates the health bar UI.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthBar;
    public int health = 100;
    private Animator animator;
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update. It finds and sets up a reference to the GameManager and the health bar UI.
    /// </summary>
    void Start()
    {
        healthBar.value = health;
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// OnTriggerEnter is called when the player collides with another collider.
    /// If the collided object is the boss, it applies damage to the player.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss") && gameManager.bossDead == false)
        {
            TakeDamage(10);
        }
    }

    /// <summary>
    /// TakeDamage is called when the player takes damage.
    /// It applies damage to the player and updates the health bar UI.
    /// If the player's health is less than or equal to 0, it calls the Die method.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        healthBar.value = health;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
            StartCoroutine(FlashRed());
        }
    }

    /// <summary>
    /// FlashRed is called when the player takes damage.
    /// It flashes the player red for a short period of time.
    /// </summary>
    IEnumerator FlashRed()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.material.color = Color.white;
    }

    /// <summary>
    /// Die is called when the player's health is less than or equal to 0.
    /// It calls the PlayerDefeated method in the GameManager.
    /// </summary>
    void Die()
    {
        gameManager.playerDead = true;
        gameManager.PlayerDefeated();
    }
}
