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
    private int maxHealth = 100;

    [SyncVar]
    private int playerHealth;

    [SyncVar]
    public int playerScore;

    [SyncVar]
    public int kills;


    // UI Elements
    public Text healthText;
    public Text scoreText;

        Material newMaterial;
    void Start()
    {
        //playerHealth = rnd.Next(50, 101);
        playerHealth = maxHealth;
        playerScore = rnd.Next(1, 10001);
        changeColor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            takeDamage(25);
        }
    }

    [ClientRpc]
    public void RpcRegisterHit()
    {
        takeDamage(50);
        Debug.Log("Player Health Decremented. Health: " + getHealth());

        if (getHealth() <= 0)
        {
            StartCoroutine(Respawn());
            setDefaults();
        }
    }
       


    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        Transform _spawn = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawn.position;
        transform.rotation = _spawn.rotation;


    }

    public void setDefaults ()
    {
        playerHealth = maxHealth;
        kills = 0;
    }

    public void setHealth(int Val)
    {
        playerHealth += Val;
    }

    public void takeDamage(int damage)
    {
        playerHealth -= damage;
        changeColor();
    }

    public int getHealth ()
    {
        return playerHealth;
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

public void changeColor()
{
        float healthPercent = (float)playerHealth / maxHealth;
        Debug.Log("playerHealth: " + playerHealth);
        Debug.Log("maxHealth: " + maxHealth);
        Debug.Log("healthPercent: " + healthPercent);
        if (healthPercent > .75)
        {
        newMaterial = Resources.Load<Material>("HealthSkins/FullHealth");
        }
       else if (healthPercent <= .75 && healthPercent > .5)
        {
            newMaterial = Resources.Load<Material>("HealthSkins/75%Health");
        }
        else if (healthPercent <= .5 && healthPercent > .25)
        {
            newMaterial = Resources.Load<Material>("HealthSkins/50%Health");
        }
        else if (healthPercent <= .25)
        {
            newMaterial = Resources.Load<Material>("HealthSkins/25%Health");
        }
        transform.GetChild(0).GetChild(0).GetComponentInChildren<Renderer>().material = newMaterial;
        transform.GetChild(0).GetChild(1).GetComponentInChildren<Renderer>().material = newMaterial;
        GetComponentInChildren<Renderer>().material = newMaterial;
    }
}