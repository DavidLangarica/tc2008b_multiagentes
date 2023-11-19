using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damage = 5;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Minion") && gameManager.minionLevelReady)
        {
            MinionHealth minionHealth = other.gameObject.GetComponent<MinionHealth>();
            if (minionHealth != null)
            {
                minionHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Boss") && gameManager.bossLevelReady)
        {
            BossHealth bossHealth = other.gameObject.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
