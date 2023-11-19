/// <summary>
/// The LookAtPlayerHealthBar class is responsible for making the health bar UI face the player.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerHealthBar : MonoBehaviour
{
    private new Transform camera;

    /// <summary>
    /// Start is called before the first frame update. It initializes the camera reference.
    /// </summary>
    void Start()
    {
        camera = Camera.main.transform;
    }

    /// <summary>
    /// LateUpdate is called after all Update methods have been called. It makes the health bar UI face the player.
    /// </summary>
    void LateUpdate()
    {
        transform.LookAt(camera);
    }
}
