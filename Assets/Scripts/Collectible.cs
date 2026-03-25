using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1; // esim. pisteet, kolikot, energia

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Lis‰‰ pisteet tai muu toiminto
            Needs inventory = other.GetComponent<Needs>();
            if (inventory != null)
            {
                inventory.addMoney(10);
            }

            // Tuhoa esine
            Destroy(gameObject);
        }
    }
}