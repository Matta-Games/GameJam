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
            PlayerMoney money = other.GetComponent<PlayerMoney>();
            if (inventory != null)
            {
                inventory.addMoney(10);
                money.Add(10);
            }

            // Tuhoa esine
            Destroy(gameObject);
        }
    }
}