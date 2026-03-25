using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public bool isConsumable = false;

    // Special effect parameters
    public bool isSundel = false;
    public bool isRedMoo = false;
    public float redMooSpeedMultiplier = 1.5f;
    public float redMooDuration = 5f;
    public float sundelHeatAmount = 0.05f;
    public float sundelDuration = 1f; // lasts 1 second

    public void UseEffect(PlayerInventory inventory)
    {
        PlayerController pc = inventory.GetComponent<PlayerController>();
        Needs needs = inventory.GetComponent<Needs>();
        if (pc == null || needs == null) return;

        if (isSundel)
        {
            inventory.StartCoroutine(ApplySundel(needs));
        }

        if (isRedMoo)
        {
            inventory.StartCoroutine(ApplyRedMoo(pc));
        }
    }

    private IEnumerator ApplySundel(Needs needs)
    {
        needs.isBeingHeated = true;

        float timer = 0f;
        while (timer < sundelDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        needs.isBeingHeated = false;
    }

    private IEnumerator ApplyRedMoo(PlayerController pc)
    {
        float originalSpeed = pc.moveSpeed;
        pc.moveSpeed *= redMooSpeedMultiplier;

        float timer = 0f;
        while (timer < redMooDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        pc.moveSpeed = originalSpeed;
    }
}