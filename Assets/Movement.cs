using Assets.Interfaces;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float speed = 12f;
    public float gravity = -20f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 4;

    private Vector3 velocity;
    private bool isGrounded;
    private IPickup pickupComponent;

    // Start is called before the first frame update
    void Start()
    {
        pickupComponent = GetComponentInChildren<IPickup>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveXZ();

        MoveY();
    }

    private void MoveY()
    {
        ApplyGravity();
        if (Input.GetButtonDown("Jump") && isGrounded && !pickupComponent.IsHoldingObject())
        {
            ApplyJump();
        }
    }

    private void ApplyJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * (-2) * gravity);
        controller.Move(velocity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        if (isGrounded && velocity.y <= 0f)
        {
            velocity.y = 0f;
        } 
        else
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void MoveXZ()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
