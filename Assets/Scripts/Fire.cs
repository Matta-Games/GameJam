using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Heat Settings")]
    public bool heat = true;
    public float triggerRadius = 5f;

    private SphereCollider triggerCollider;

    void Awake()
    {
        triggerCollider = GetComponent<SphereCollider>();

        if (triggerCollider == null)
        {
            triggerCollider = gameObject.AddComponent<SphereCollider>();
        }

        triggerCollider.isTrigger = true;
        triggerCollider.radius = triggerRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!heat) return;

        // Only affect objects tagged "Player"
        if (other.CompareTag("Player"))
        {
            Needs stats = other.GetComponent<Needs>();
            if (stats != null)
            {
                stats.isBeingHeated = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Needs stats = other.GetComponent<Needs>();
            if (stats != null)
            {
                stats.isBeingHeated = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = heat ? Color.red : Color.gray;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}