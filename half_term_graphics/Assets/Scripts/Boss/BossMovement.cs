/// <summary>
/// The BossMovement class controls the movement of the boss character in the game.
/// It manages the boss's position and orientation in relation to the player.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    private GameObject player;

    /// <summary>
    /// Start is called before the first frame update. It finds and sets up a reference to the player GameObject.
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Update is called once per frame and handles the movement and rotation of the boss.
    /// The boss will face and move towards the player unless it is within a certain distance.
    /// </summary>
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);

        if (Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            transform.Translate(Vector3.zero);
        }
        else
        {
            transform.LookAt(player.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * 2);
        }

        transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }
}
