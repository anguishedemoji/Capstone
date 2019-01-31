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
    public int playerScore;

    // UI Elements
    public Text healthText;
    public Text scoreText;

    void Start()
    {
        playerHealth = rnd.Next(1, 101);
        playerScore = rnd.Next(1, 10001);
        setHealthText();
        setScoreText();
    }

    void Update()
    {

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
