using UnityEngine;

public class loop : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Units per second the object moves to the left.")]
    [SerializeField] private float speed = 2f;

    [Tooltip("Distance after which the object resets or wraps.")]
    [SerializeField] private float resetDistance = 10f;

    [Tooltip("When true, 'left' is relative to the object's local space; otherwise world left (negative X).")]
    [SerializeField] private bool useLocalSpace = false;

    [Tooltip("If true the object will wrap forward by `resetDistance` (smooth continuous loop). If false it will snap back to the start position.")]
    [SerializeField] private bool wrapInsteadOfReset = true;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // direction for left movement (world or local)
        Vector3 leftDir = useLocalSpace ? -transform.right : Vector3.left;

        // move
        transform.position += leftDir * speed * Time.deltaTime;

        // check travelled distance from the stored start position
        float travelled = Vector3.Distance(startPosition, transform.position);
        if (travelled >= Mathf.Max(0.0001f, resetDistance))
        {
            if (wrapInsteadOfReset)
            {
                // move forward opposite to leftDir by resetDistance to create a seamless loop
                Vector3 wrapOffset = -leftDir.normalized * resetDistance;
                transform.position += wrapOffset;
                // shift startPosition as well so distance measurement continues correctly
                startPosition += wrapOffset;
            }
            else
            {
                // snap back to initial start position
                transform.position = startPosition;
            }
        }
    }

    // keep values sane in the inspector
    void OnValidate()
    {
        if (resetDistance < 0f) resetDistance = 0f;
        if (speed < 0f) speed = 0f;
    }
}
