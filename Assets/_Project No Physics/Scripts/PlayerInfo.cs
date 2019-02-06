using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{
    // Player info
    [SyncVar]
    private int maxHealth = 100;

    [SyncVar]
    private int playerHealth;

    [SyncVar]
    public int playerScore = 0;

    [SyncVar]
    public int kills;

    // Camera Shake
    public float camShakeDuration = 0.2f;
    public float camShakeMagnitude = 0.5f;
    private Camera playerCam;

    //Cube Object
    private PlayerGameObject playerObject;

    // UI Elements
    public Text healthText;
    public Text scoreText;

    Material newMaterial;

    void Start()
    {
        playerHealth = maxHealth;
        playerCam = GetComponentInChildren<Camera>();
        playerObject = GetComponent<PlayerGameObject>();
        ChangeColor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RpcRegisterHit();
        }
    }

    [ClientRpc]
    public void RpcRegisterHit()
    {
        TakeDamage(25);
        ChangeColor();
        StartCoroutine(ShakeCam());

        if (GetHealth() <= 0)
        {
            ChangeColor();
            playerObject.DeathMove();
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        Transform _spawn = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawn.position;
        transform.rotation = _spawn.rotation;
        SetDefaults();
    }

    public void SetDefaults ()
    {
        playerHealth = maxHealth;
        kills = 0;
        ChangeColor();
    }

    public void SetHealth(int Val)
    {
        playerHealth += Val;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
    }

    public void IncreaseScore(int val)
    {
        playerScore += val;
    }

    private IEnumerator ShakeCam()
    {
        float elapsed = 0.0f;

        Vector3 originalCamPosition = playerCam.transform.localPosition;

        while (elapsed < camShakeDuration)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / camShakeDuration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // Map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= camShakeMagnitude * damper;
            y *= camShakeMagnitude * damper;

            playerCam.transform.localPosition = new Vector3(x, y, originalCamPosition.z);

            yield return null;
        }

        playerCam.transform.localPosition = originalCamPosition;
    }

    public int GetHealth ()
    {
        return playerHealth;
    }

    // GUI 
    void OnGUI()
    {
        SetHealthText();
        SetScoreText();
    }

    // Create text strings to be displayed in UI
    void SetHealthText()
    {
        healthText.text = "Health: " + playerHealth.ToString();
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void ChangeColor()
    {
        float healthPercent = (float)playerHealth / maxHealth;
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
        else if (healthPercent <= .25 && healthPercent > 0 )
        {
            newMaterial = Resources.Load<Material>("HealthSkins/25%Health");
        }
        else if (healthPercent <= 0)
        {
            newMaterial = Resources.Load<Material>("HealthSkins/0%Health");
        }
        transform.GetChild(0).GetChild(0).GetComponentInChildren<Renderer>().material = newMaterial;
        transform.GetChild(0).GetChild(1).GetComponentInChildren<Renderer>().material = newMaterial;
        GetComponentInChildren<Renderer>().material = newMaterial;
    }
}