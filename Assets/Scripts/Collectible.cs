using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1; // esim. pisteet, kolikot, energia
    public AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Lis‰‰ pisteet tai muu toiminto
            Needs inventory = other.GetComponent<Needs>();
            PlayerMoney money = other.GetComponent<PlayerMoney>();
            if (inventory != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
                inventory.addMoney(10);
                money.Add(10);
            }

            // Tuhoa esine
            Destroy(gameObject);
        }
    }
}