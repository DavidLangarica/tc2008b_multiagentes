/// <summary>
/// The FollowPlayer class is responsible for making the camera follow the player.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 0, 4);

    /// <summary>
    /// Start is called before the first frame update. It sets the camera's position to be behind the player.
    /// </summary>
    void Update()
    {
        Rotate();
        transform.position = player.transform.position - transform.forward * offset.magnitude;
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
    }

    /// <summary>
    /// Rotates the camera around the player.
    /// </summary>
    void Rotate()
    {
        float rotationSpeed = 300.0f;

        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(player.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        transform.RotateAround(player.transform.position, Vector3.up, mouseX);
        transform.RotateAround(player.transform.position, transform.right, -mouseY);

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = 0;

        if (rotation.x > 26 && rotation.x < 345)
        {
            if (rotation.x > 180)
            {
                rotation.x = 345;
            }
            else
            {
                rotation.x = 26;
            }
        }
        transform.rotation = Quaternion.Euler(rotation);
    }

}