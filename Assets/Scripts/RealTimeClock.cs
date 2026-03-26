using UnityEngine;
using TMPro;
using System;

public class RealTimeClock : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    void Update()
    {
        DateTime now = DateTime.Now;
        clockText.text = now.ToString("HH:mm");
    }
}