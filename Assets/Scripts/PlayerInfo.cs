using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{
    // Initialize random number generator
    System.Random rnd = new System.Random();

    // Player info
    [SyncVar]
    public int playerHealth;
    [SyncVar]
    public int playerScore;

    // UI Elements
    public Text healthText;
    public Text scoreText;

    void Start()
    {
        playerHealth = rnd.Next(50, 101);
        playerScore = rnd.Next(1, 10001);
    }

    void Update()
    {

    }

    public void setHealth(int Val)
    {
        playerHealth += Val;
    }

    // GUI 
    void OnGUI()
    {
        setHealthText();
        setScoreText();
    }

    // Create text strings to be displayed in UI
    void setHealthText()
    {
        healthText.text = "Health: " + playerHealth.ToString();
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }
}
