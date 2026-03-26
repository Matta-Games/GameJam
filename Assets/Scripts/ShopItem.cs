using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int price = 10;
    public Item item;

    private Needs playerNeeds;
    private PlayerInventory inventory;

    void Start()
    {
        playerNeeds = FindObjectOfType<Needs>();
        inventory = FindObjectOfType<PlayerInventory>();
    }

    void OnMouseDown()
    {
        if (playerNeeds == null || inventory == null) return;

        if (playerNeeds.Spend(price))
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
        else
        {
            Debug.Log("Not enough money");
        }
    }
}