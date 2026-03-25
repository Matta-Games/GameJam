using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxSlots = 10;
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public int selectedSlot = 0;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject currentSpawnedItem;

    void Update()
    {
        // Number selection 1-9
        for (int i = 0; i < Mathf.Min(9, maxSlots); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                Debug.Log("Selected: " + GetSelectedItemName());
                SpawnSelectedItem();
            }
        }

        // Use the selected item
        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }

    public bool AddItem(Item newItem, int qty = 1)
    {
        foreach (InventorySlot slot in inventory)
        {
            if (slot.item == newItem)
            {
                slot.quantity += qty;
                return true;
            }
        }

        if (inventory.Count >= maxSlots)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        inventory.Add(new InventorySlot(newItem, qty));
        return true;
    }

    public void UseSelectedItem()
    {
        if (selectedSlot >= inventory.Count) return;
        InventorySlot slot = inventory[selectedSlot];
        if (slot.item == null) return;

        // Apply item effect
        slot.item.UseEffect(this);

        if (slot.item.isConsumable)
        {
            slot.quantity--;

            if (slot.quantity <= 0)
            {
                inventory.RemoveAt(selectedSlot);
                if (currentSpawnedItem != null)
                {
                    Destroy(currentSpawnedItem);
                    currentSpawnedItem = null;
                }
            }

            Debug.Log("Used consumable: " + slot.item.itemName);
        }
        else
        {
            SpawnSelectedItem(); // equipable item
            Debug.Log("Equipped: " + slot.item.itemName);
        }
    }

    public string GetSelectedItemName()
    {
        if (selectedSlot < inventory.Count)
            return inventory[selectedSlot].item.itemName;
        return "Empty";
    }

    void SpawnSelectedItem()
    {
        if (currentSpawnedItem != null)
            Destroy(currentSpawnedItem);

        if (selectedSlot >= inventory.Count) return;
        InventorySlot slot = inventory[selectedSlot];
        if (slot.item == null || slot.item.prefab == null) return;

        currentSpawnedItem = Instantiate(slot.item.prefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public InventorySlot(Item newItem, int qty = 1)
    {
        item = newItem;
        quantity = qty;
    }
}