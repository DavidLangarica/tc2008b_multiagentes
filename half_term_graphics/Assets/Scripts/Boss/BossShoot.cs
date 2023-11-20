/// <summary>
/// The BossAttackPatterns class manages the different attack patterns of the boss character.
/// It selects and executes attack patterns based on the game state and the boss's behavior.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackPatterns : MonoBehaviour
{
    public GameObject projectile;
    public bool isRandom = true;

    private Transform player;
    private int patternIndex = 0;
    private GameManager gameManager;
    private int numberOfPatterns = 4;
    private bool bossLevelStarted = false;

    /// <summary>
    /// Start is called before the first frame update. It initializes references and sets the initial state.
    /// </summary>
    void Start()
    {
        bossLevelStarted = false;
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// LateUpdate is called after all Update methods have been called. This is where it checks if the boss level is ready to start the attack pattern routine.
    /// </summary>
    private void LateUpdate()
    {
        if (gameManager.bossLevelReady && !bossLevelStarted)
        {
            StartCoroutine(ShotRoutine());
            bossLevelStarted = true;
        }
    }

    /// <summary>
    /// Coroutine that manages the sequence of attack patterns. It selects patterns either randomly or sequentially.
    /// </summary>
    IEnumerator ShotRoutine()
    {
        int[] weightedPatterns = new int[] { 0, 0, 0, 1, 1, 2, 2, 3 };
        while (!gameManager.bossDead)
        {
            if (isRandom)
            {
                patternIndex = weightedPatterns[Random.Range(0, weightedPatterns.Length)];
            }
            switch (patternIndex)
            {
                case 0:
                    StartCoroutine(SporesExplosionPattern());
                    break;
                case 1:
                    StartCoroutine(CirclePattern());
                    break;
                case 2:
                    StartCoroutine(SpiralPattern());
                    break;
                case 3:
                    StartCoroutine(CascadePattern());
                    break;
            }

            if (!isRandom)
            {
                if (patternIndex == numberOfPatterns - 1)
                {
                    patternIndex = 0;
                }
                else
                {
                    patternIndex = patternIndex + 1;
                }
            }

            float randomDelay = Random.Range(3.5f, 6.7f);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    /// <summary>
    /// Coroutine for the Spores Explosion attack pattern. It launches a large number of projectiles in a spherical pattern.
    /// </summary>
    IEnumerator SporesExplosionPattern()
    {
        int numProjectiles = 600;
        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = i * Mathf.PI * 2 / numProjectiles;
            float height = Mathf.Sin(i * Mathf.PI * 2 / 20) * 2;
            float width = Mathf.Cos(i * Mathf.PI * 2 / 20) * 5;
            Vector3 direction = new Vector3(width * Mathf.Cos(angle), height, width * Mathf.Sin(angle));
            GameObject proj = Instantiate(projectile, transform.position + direction, Quaternion.identity);
            proj.GetComponent<Rigidbody>().velocity = direction.normalized * 10f;
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine for the Circle attack pattern. It launches projectiles in a circular pattern around the boss.
    /// </summary>
    IEnumerator CirclePattern()
    {
        int numProjectiles = 50;
        int numWaves = 6;
        float delayBetweenWaves = 0.8f;

        for (int wave = 0; wave < numWaves; wave++)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                float angle = i + wave * Mathf.PI * 2 / numProjectiles;
                Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                proj.GetComponent<Rigidbody>().velocity = direction.normalized * 15f;
            }
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }

    /// <summary>
    /// Coroutine for the Spiral attack pattern. It launches projectiles in a spiral pattern towards the player.
    /// </summary>
    IEnumerator SpiralPattern()
    {
        int numProjectiles = 500;
        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = i * Mathf.PI * 2 / numProjectiles;
            Vector3 direction = new Vector3(Mathf.Cos(angle) * Mathf.Sin(i), Mathf.Cos(i), Mathf.Sin(angle) * Mathf.Sin(i)).normalized;
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody>().velocity = (direction + (player.position - transform.position).normalized * 0.5f).normalized * 15f;
            yield return null;
        }
    }

    /// <summary>
    /// Coroutine for the Cascade attack pattern. It fires projectiles in a cube pattern that expands and contracts.
    /// </summary>
    IEnumerator CascadePattern()
    {
        int cubeSize = 3;
        float expansionDuration = 2.0f;
        float contractionDuration = 4.0f;
        float timeStep = 0.2f;

        for (float t = 0; t <= 1; t += timeStep / expansionDuration)
        {
            FireProjectilesInCubePattern(Mathf.Lerp(1, cubeSize, t));
            yield return new WaitForSeconds(timeStep);
        }

        for (float t = 1; t >= 0; t -= timeStep / contractionDuration)
        {
            FireProjectilesInCubePattern(Mathf.Lerp(1, cubeSize, t));
            yield return new WaitForSeconds(timeStep);
        }
    }

    /// <summary>
    /// Fires projectiles in a cube pattern with a specified scale.
    /// </summary>
    /// <param name="scale">The scale factor for the cube pattern.</param>
    void FireProjectilesInCubePattern(float scale)
    {
        int numProjectilesPerPoint = 1;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    Vector3 point = new Vector3(x, y, z) * scale;
                    for (int i = 0; i < numProjectilesPerPoint; i++)
                    {
                        FireSingleProjectileFromPoint(point);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Fires a single projectile from a specified point.
    /// </summary>
    /// <param name="point">The starting point for the projectile.</param>
    void FireSingleProjectileFromPoint(Vector3 point)
    {
        Vector3 startPoint = transform.position + point;
        Vector3 direction = (player.position - startPoint).normalized + Random.insideUnitSphere * 0.05f;
        GameObject proj = Instantiate(projectile, startPoint, Quaternion.identity);
        proj.GetComponent<Rigidbody>().velocity = direction * 15f;
    }

}