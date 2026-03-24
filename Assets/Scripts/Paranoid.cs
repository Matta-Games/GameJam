using UnityEngine;

public class Paranoid : MonoBehaviour
{
    [Header("Paranoia Settings")]
    public bool causeParanoia = true;
    public float triggerRadius = 5f;
    public float paranoiaIncreasePerSecond = 0.02f;

    private SphereCollider triggerCollider;
    private Needs playerNeeds;

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
        if (!causeParanoia) return;

        if (other.CompareTag("Player"))
        {
            playerNeeds = other.GetComponent<Needs>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNeeds = null;
        }
    }

    private void Update()
    {
        if (!causeParanoia || playerNeeds == null) return;

        playerNeeds.paranoiaPercent += paranoiaIncreasePerSecond * Time.deltaTime;
        playerNeeds.paranoiaPercent = Mathf.Clamp01(playerNeeds.paranoiaPercent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = causeParanoia ? Color.magenta : Color.gray;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}