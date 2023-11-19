/// <summary>
/// The MinionBehaviour class controls the movement and attack of the minion character in the game.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaviour : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float moveSpeed = 1f;
    public float attackInterval = 2f;
    public float waypointUpdateInterval = 8f;
    public float waypointRadius = 8f;
    public float projectileSpeed = 10f;

    private float lastAttackTime;
    private float lastWaypointUpdateTime;
    private GameObject player;
    private Vector3 waypoint;

    /// <summary>
    /// Start is called before the first frame update. It finds and sets up a reference to the player GameObject.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateWaypoint();
    }

    /// <summary>
    /// Update is called once per frame and handles the movement and attack of the minion.
    /// The minion will move towards a random waypoint around the player and shoot projectiles at the player.
    /// </summary>
    void Update()
    {
        if (Time.time - lastWaypointUpdateTime > waypointUpdateInterval)
        {
            UpdateWaypoint();
        }

        MoveTowards(waypoint);

        if (Time.time - lastAttackTime > attackInterval)
        {
            ShootProjectile();
            lastAttackTime = Time.time;
        }
    }

    /// <summary>
    /// Updates the waypoint to a random position around the player.
    /// </summary>
    void UpdateWaypoint()
    {
        waypoint = GetRandomWaypointAroundPlayer();
        lastWaypointUpdateTime = Time.time;
    }

    /// <summary>
    /// Gets a random waypoint around the player.
    /// </summary>
    Vector3 GetRandomWaypointAroundPlayer()
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float x = player.transform.position.x + Mathf.Cos(angle) * waypointRadius;
        float z = player.transform.position.z + Mathf.Sin(angle) * waypointRadius;

        return new Vector3(x, player.transform.position.y, z);
    }

    /// <summary>
    /// Moves the minion towards the target position.
    /// </summary>
    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float distanceToWaypoint = Vector3.Distance(transform.position, target);

        if (distanceToWaypoint > 1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        }
    }

    /// <summary>
    /// Shoots a projectile at the player.
    /// </summary>
    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.transform.LookAt(player.transform);
        projectile.transform.position = new Vector3(projectile.transform.position.x, 1.5f, projectile.transform.position.z);
        projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;
    }
}
