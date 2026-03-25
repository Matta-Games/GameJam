using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int price = 10;
    public Item item;

    private PlayerMoney playerMoney;
    private PlayerInventory inventory;

    void Start()
    {
        playerMoney = FindObjectOfType<PlayerMoney>();
        inventory = FindObjectOfType<PlayerInventory>();
    }

    void OnMouseDown()
    {
        if (playerMoney == null || inventory == null) return;

        if (playerMoney.Spend(price))
        {
            if (inventory.AddItem(item))
            {
                Debug.Log("Bought: " + item.itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full, cannot buy item");
            }
        }
    }
}