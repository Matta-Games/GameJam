using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    public int damageAmount = 20;
    public float attackRange = 2f;
    public LayerMask enemyLayer;

    [Header("Animation")]
    public Animator animator;
    public string swingTrigger = "Swing";

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }

    void Attack()
    {
        // play swing animation
        if (animator != null)
        {
            animator.SetTrigger(swingTrigger);
        }

        // raycast forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyLayer))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }
    }
}