using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int money = 100;

    public bool Spend(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
    }

    public void Add(int amount)
    {
        money += amount;
    }
}