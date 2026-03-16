using UnityEngine;

public class Sled : MonoBehaviour
{
    [Header("Sled Movement")]
    public float acceleration = 25f;
    public float maxSpeed = 15f;
    public float deceleration = 8f;
    public float rotationSpeed = 90f;

    [Header("Sled Mounting")]
    public Transform playerSeatPosition;
    public float mountRange = 3f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float gravityScale = 2f;

    private Rigidbody rb;
    private PlayerController mountedPlayer;
    private Transform playerCamera;
    private bool playerMounted = false;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 5f;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.5f;

        if (playerSeatPosition == null)
        {
            Debug.LogError("Player seat position not assigned!");
        }
    }

    void Update()
    {
        if (playerMounted && playerCamera != null)
        {
            HandleSledInput();
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        MoveSled();
        ApplyGravity();
    }

    void HandleSledInput()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotateInput = Input.GetAxis("Horizontal");

        // W/S movement
        if (moveInput > 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (moveInput < 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, -maxSpeed * 0.5f, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // A/D rotation - only around world Y axis (turning left/right)
        if (rotateInput != 0f)
        {
            transform.RotateAround(transform.position, Vector3.up, rotateInput * rotationSpeed * Time.deltaTime);
        }
    }

    void MoveSled()
    {
        if (playerCamera == null) return;

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            Vector3 cameraForward = playerCamera.forward;
            cameraForward.y = 0f;
            cameraForward = cameraForward.normalized;

            Vector3 movement = cameraForward * currentSpeed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void ApplyGravity()
    {
        rb.linearVelocity += Vector3.down * 9.81f * gravityScale * Time.deltaTime;
    }

    public void MountPlayer(PlayerController player)
    {
        playerMounted = true;
        mountedPlayer = player;
        playerCamera = player.cameraTransform;
    }

    public void DismountPlayer()
    {
        playerMounted = false;
        mountedPlayer = null;
        playerCamera = null;
        currentSpeed = 0f;
    }

    public bool IsPlayerMounted()
    {
        return playerMounted;
    }
}