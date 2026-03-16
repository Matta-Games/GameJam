using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;

    [Header("Head Bob")]
    public Transform cameraHolder;
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;

    private Rigidbody rb;
    private Vector3 input;

    private float bobTimer = 0f;
    private Vector3 originalCamPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // estää kaatumisen

        originalCamPos = cameraHolder.localPosition;
    }

    void Update()
    {
        // Liike-input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        input = new Vector3(x, 0f, z).normalized;

        HandleHeadBob();
    }

    void FixedUpdate()
    {
        // Liikuta rigidbodyä
        Vector3 move = transform.TransformDirection(input) * moveSpeed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }

    void HandleHeadBob()
    {
        bool isMoving = input.magnitude > 0.1f;

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