using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Head Bob")]
    public Transform cameraHolder;
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private float bobTimer = 0f;
    private Vector3 originalCamPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCamPos = cameraHolder.localPosition;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Head bob
        HandleHeadBob(x, z);
    }

    void HandleHeadBob(float x, float z)
    {
        bool isMoving = (x != 0 || z != 0) && isGrounded;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmount;

            cameraHolder.localPosition = new Vector3(
                originalCamPos.x,
                originalCamPos.y + bobOffset,
                originalCamPos.z
            );
        }
        else
        {
            cameraHolder.localPosition = Vector3.Lerp(
                cameraHolder.localPosition,
                originalCamPos,
                Time.deltaTime * bobSpeed
            );
        }
    }
}