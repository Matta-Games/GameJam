using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public Transform cameraTransform;
    public float headBobSpeed = 5f;
    public float headBobAmount = 0.05f;
    public float mouseSensitivity = 2f;
    public Rigidbody rb;
    public bool cameraLocked = true;
    [HideInInspector] public bool inShopMode = false;

    private float defaultYPos;
    private float headBobTimer;
    private float rotationX = 0f;
    private float _gravity = 9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;

    // Sled interaction
    private Sled nearestSled;
    private float sledDetectionRange = 5f;
    private bool isMountedOnSled = false;

    // Slow effect variables
    private float slowAmount = 0f;
    private float slowDuration = 0f;
    private float slowTimer = 0f;
    public bool interactPressed;

    [Header("Audio")]
    public AudioSource voiceSource;
    public AudioClip[] swearLines;
    public KeyCode swearKey = KeyCode.V;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        defaultYPos = cameraTransform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        interactPressed = Input.GetKeyDown(KeyCode.F);

        if (!inShopMode)
        {
            FindNearestSled();
            HandleMovement();
            HandleMouseLook();
            HandleSlowEffect();
        }

        if (interactPressed)
        {
            // NPC scripts can read this
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isMountedOnSled && nearestSled != null)
                MountSled(nearestSled);
            else if (isMountedOnSled)
                DismountSled();
        }

        ApplyGravity();

        if (Input.GetKeyDown(swearKey))
            PlaySwear();
    }

    void FindNearestSled()
    {
        Sled[] sleds = FindObjectsOfType<Sled>();
        nearestSled = null;
        float nearestDistance = sledDetectionRange;

        foreach (Sled sled in sleds)
        {
            float distance = Vector3.Distance(transform.position, sled.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestSled = sled;
            }
        }
    }
    void PlaySwear()
    {
        if (voiceSource == null || swearLines.Length == 0) return;

        if (!voiceSource.isPlaying)
        {
            int index = Random.Range(0, swearLines.Length);
            voiceSource.clip = swearLines[index];
            voiceSource.Play();
        }
    }

    void MountSled(Sled sled)
    {
        isMountedOnSled = true;

        // Make player kinematic so it follows sled perfectly
        rb.isKinematic = true;

        // Parent player to sled seat
        transform.SetParent(sled.playerSeatPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sled.MountPlayer(this);
    }

    void DismountSled()
    {
        if (nearestSled != null)
        {
            isMountedOnSled = false;

            // Unparent player from sled
            transform.SetParent(null);

            // Move player slightly away from sled
            transform.position += transform.parent != null ? Vector3.zero : nearestSled.transform.right * 2f;

            // Make player dynamic again
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;

            nearestSled.DismountPlayer();
            nearestSled = null;
        }
    }

    void ApplyGravity()
    {
        rb.linearVelocity += Vector3.down * _gravity * gravityMultiplier * Time.deltaTime;
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * moveX + transform.forward * moveZ;

        Vector3 horizontalVelocity = new Vector3(movement.x * moveSpeed * (1 - slowAmount), rb.linearVelocity.y, movement.z * moveSpeed * (1 - slowAmount));

        rb.linearVelocity = horizontalVelocity;

        ApplyHeadBob(movement.magnitude > 0);
    }

    void HandleMouseLook()
    {
        if (!cameraLocked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (!isMountedOnSled)
        {
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void ApplyHeadBob(bool isMoving)
    {
        if (!isMoving)
        {
            headBobTimer = 0;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultYPos, cameraTransform.localPosition.z);
            return;
        }

        headBobTimer += Time.deltaTime * headBobSpeed;
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultYPos + Mathf.Sin(headBobTimer) * headBobAmount, cameraTransform.localPosition.z);
    }

    public void ApplySlow(float slowAmount, float slowDuration)
    {
        this.slowAmount = slowAmount;
        this.slowDuration = slowDuration;
        slowTimer = slowDuration;
    }

    private void HandleSlowEffect()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
        }
        else
        {
            slowAmount = 0f;
        }
    }

    public bool IsMountedOnSled()
    {
        return isMountedOnSled;
    }
}