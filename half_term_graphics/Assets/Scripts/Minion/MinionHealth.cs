/// <summary>
/// The MinionHealth class is responsible for handling the health of minions.
/// It handles damage taken by minions and updates the health bar UI.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionHealth : MonoBehaviour
{
    public int health = 100;
    private GameManager gameManager;
    private Slider heallthBar;

    /// <summary>
    /// Start is called before the first frame update. It finds and sets up a reference to the GameManager and the health bar UI.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        heallthBar = GetComponentInChildren<Slider>();
    }

    /// <summary>
    /// TakeDamage is called when the minion takes damage.
    /// It applies damage to the minion and updates the health bar UI.
    /// If the minion's health is less than or equal to 0, it calls the Die method.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (health <= 0) return;

        health -= damage;
        heallthBar.value = health;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Die is called when the minion's health is less than or equal to 0.
    /// It calls the MinionDefeated method in the GameManager and destroys the minion GameObject.
    /// </summary>
    void Die()
    {
        gameManager.MinionDefeated();
        Destroy(gameObject);
    }
}