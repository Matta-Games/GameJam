using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRange = 12f;
    public float attackRange = 2f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;
    public float bodyTempDamage = 0.15f; // how much heat to remove

    private Transform player;
    private Needs playerNeeds;
    private float attackTimer;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            playerNeeds = playerObj.GetComponent<Needs>();
        }
    }

    void Update()
    {
        if (player == null) return;

        LookAtPlayer();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            MoveTowardsPlayer();

            if (distance <= attackRange)
            {
                Attack();
            }
        }

        attackTimer -= Time.deltaTime;
    }

    void MoveTowardsPlayer()
    {
        // move forward in the direction the enemy is facing
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // keep enemy upright

        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                8f * Time.deltaTime
            );
        }
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

        Debug.Log("Enemy attacked — body temp reduced");
    }
}