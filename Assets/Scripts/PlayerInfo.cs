using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    // Initialize random number generator
    System.Random rnd = new System.Random();

    // Player info
    public int playerHealth;

    // UI Elements
    public Text healthText;

    void Start()
    {
        playerHealth = rnd.Next(1, 101);
        setHealthText();
    }

    void Update()
    {

    }

    // Create text string to be displayed in UI
    void setHealthText()
    {
        healthText.text = "Health: " + playerHealth.ToString();
    }
}
