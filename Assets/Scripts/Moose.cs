using UnityEngine;

public class Moose : MonoBehaviour
{
    public Animator animator;

    // track XZ-only position to ignore vertical movement (Y)
    private Vector3 lastPositionXZ;

    [SerializeField] private float speedDeadZone = 0.05f;
    [SerializeField] private float dampTime = 0.1f;

    void Start()
    {
        lastPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    void Update()
    {
        // build XZ-only positions so Y changes are ignored
        Vector3 currentXZ = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 delta = currentXZ - lastPositionXZ;
        float velocity = delta.magnitude / Mathf.Max(Time.deltaTime, 1e-6f);

        float targetSpeed = velocity > speedDeadZone ? velocity : 0f;

        animator.SetFloat("Speed", targetSpeed, dampTime, Time.deltaTime);

        lastPositionXZ = currentXZ;
    }
}