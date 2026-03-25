using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    public int damageAmount = 20;          // Kuinka paljon damagea kirves tekee
    public float attackRange = 2f;         // Kuinka kauas kirves ylt‰‰
    public LayerMask enemyLayer;           // Layer, jossa viholliset ovat

    void Update()
    {
        if (Input.GetMouseButtonDown(1))   // 1 = oikea hiiren nappi
        {
            Attack();
        }
    }

    void Attack()
    {
        // Raycast pelaajan edest‰
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
