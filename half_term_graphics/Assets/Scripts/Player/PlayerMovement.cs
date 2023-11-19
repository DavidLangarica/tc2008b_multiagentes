/// <summary>
/// The PlayerMovement class is responsible for handling the player's movement and attacks.
//  Based on https://www.youtube.com/watch?v=qc0xU2Ph86Q
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public GameObject projectile;
    private Vector3 moveDirection;
    private Vector3 velocity;
    public float gravity;
    public float jumpHeight;
    public float spread;
    public float shootForce;
    public Camera mainCam;
    public Transform attackPoint;

    private CharacterController controller;
    private Animator animator;
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update. It initializes the CharacterController, Animator, and GameManager references.
    /// </summary>
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Update is called once per frame and handles the player's movement and attack.
    /// </summary>
    private void Update()
    {
        if (gameManager.playerDead)
        {
            animator.SetFloat("Speed", 2.5f);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -0.7f, transform.position.z), 0.1f);
        }
        else if (gameManager.bossDead)
        {
            animator.SetFloat("Speed", 3f);
        }
        else
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.S))
            {
                Attack();
            }
        }
    }

    /// <summary>
    /// Move is called once per frame and handles the player's movement.
    /// </summary>
    private void Move()
    {
        HandleGravity();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * -Input.GetAxis("Vertical") + right * -Input.GetAxis("Horizontal");
        desiredMoveDirection.Normalize();

        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.15F);
        }

        if (controller.isGrounded)
        {
            HandleGroundedMovement();
            HandleJumpInput();
        }

        moveDirection = desiredMoveDirection * moveSpeed;
        controller.Move(moveDirection * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles the player's gravity.
    /// </summary>
    private void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
    }

    /// <summary>
    /// Handles the player's movement while on the ground.
    /// </summary>
    private void HandleGroundedMovement()
    {
        if (moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else
            {
                Walk();
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), 0.15F);

        }
        else
        {
            Idle();
        }
    }

    /// <summary>
    /// Handles the player's jump input.
    /// </summary>
    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// Handles the player's idle animation.
    /// </summary>
    private void Idle()
    {
        animator.SetFloat("Speed", 0f);
    }

    /// <summary>
    /// Handles the player's walk movement and animation.
    /// </summary>
    private void Walk()
    {
        moveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f);
    }

    /// <summary>
    /// Handles the player's run movement and animation.
    /// </summary>
    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1f);
    }

    /// <summary>
    /// Handles the player's attack animation.
    /// </summary>
    private void Attack()
    {
        Vector3 mousePosition = Input.mousePosition;

        animator.SetTrigger("Attack");

        for (int i = 0; i < spread; i++)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Shoots a projectile from the player's attack point.
    /// </summary>
    private void Shoot()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000);
        }

        GameObject bullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (targetPoint - attackPoint.position).normalized;
            rb.velocity = direction * shootForce;
        }
    }

    /// <summary>
    /// Handles the player's jump movement.
    /// </summary>
    private void Jump()
    {
        moveSpeed = walkSpeed;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        moveDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        moveDirection.Normalize();

        moveDirection *= moveSpeed;

        animator.SetTrigger("Jump");
    }
}
