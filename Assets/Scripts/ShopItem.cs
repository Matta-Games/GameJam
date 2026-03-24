using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int price = 10;
    public string itemName;

    private PlayerMoney playerMoney;

    void Start()
    {
        playerMoney = FindObjectOfType<PlayerMoney>();
    }

    void OnMouseDown()
    {
        if (playerMoney == null) return;

        if (playerMoney.Spend(price))
        {
            GiveItemToPlayer();
            Destroy(gameObject);
        }
    }

    void GiveItemToPlayer()
    {
        Debug.Log("Bought: " + itemName);
        // You can later hook inventory here
    }
}