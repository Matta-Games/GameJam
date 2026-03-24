using UnityEngine;

public class SecretCode : MonoBehaviour
{
    private string typed = "";
    private string secret = "jyrki";

    void Update()
    {
        // Kaikki näppäimistön merkit
        foreach (char c in Input.inputString)
        {
            typed += c;

            // Tarkistetaan sisältääkö kirjoitettu teksti salasanan
            if (typed.ToLower().Contains(secret))
            {
                Debug.Log("VOITIT PELIN!");
                // tähän voit laittaa pelin voittamisen logiikan
            }

            // Estetään tekstin kasvaminen loputtomasti
            if (typed.Length > 20)
                typed = typed.Substring(typed.Length - 20);
        }
    }
}
