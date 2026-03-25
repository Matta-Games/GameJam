using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Ranges")]
    public float detectionRange = 12f;
    public float attackRange = 2f;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    public float bodyTempDamage = 0.1f;

    private float attackTimer;
    private Needs playerNeeds;

    void Start()
    {
        if (player != null)
            playerNeeds = player.GetComponent<Needs>();

        // Fix model facing
        transform.Rotate(0f, 90f, 0f); // tweak 90/270 until it looks correct
    }

    void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            LookAtPlayer();

            if (distance > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                Attack();
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {
        if (attackTimer > 0f) return;

        attackTimer = attackCooldown;

        if (playerNeeds != null)
        {
            playerNeeds.bodyTempPercent -= bodyTempDamage;
            playerNeeds.bodyTempPercent = Mathf.Clamp01(playerNeeds.bodyTempPercent);
        }
    }
}