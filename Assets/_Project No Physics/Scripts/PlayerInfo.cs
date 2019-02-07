using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfo : NetworkBehaviour
{
    // Player info
    private int maxHealth = 100;

    //[SyncVar]  TODO: Change this RPC to SynVar
    private int playerHealth;

    [SyncVar]
    private bool death;

    [SyncVar]
    public int playerScore = 0;

    //[SyncVar]
    public int kills;

    // Camera Shake
    public float camShakeDuration = 0.2f;
    public float camShakeMagnitude = 0.5f;
    private Camera playerCam;

    private Vector3 camRelativePosition;

    //Cube Object
    private PlayerGameObject playerObject;

    // UI Elements
    public Text healthText;
    public Text scoreText;
    public Text playerNameText;
    public Text playerLabelText;    // text displayed over player's head in game

    Material newMaterial;

    void Start()
    {
        SetDeath(false);
        playerHealth = maxHealth;
        playerCam = GetComponentInChildren<Camera>();
        playerObject = GetComponent<PlayerGameObject>();
        camRelativePosition = playerCam.transform.localPosition;
        ChangeColor();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        uint playerId = GetComponent<NetworkIdentity>().netId.Value;
        playerLabelText.text = "Player " + playerId;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        uint playerId = GetComponent<NetworkIdentity>().netId.Value;
        if (hasAuthority)   // display player's id
        {
            playerNameText.text = "Player " + playerId;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
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
            SetDeath(true);
            ChangeColor();
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        Transform _spawn = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawn.position;
        transform.rotation = _spawn.rotation;
        playerCam.transform.localPosition = camRelativePosition;
        SetDefaults();
    }

    public void SetDefaults ()
    {
        playerHealth = maxHealth;
        kills = 0;
        ChangeColor();
        SetDeath(false);
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

        //Vector3 originalCamPosition = playerCam.transform.localPosition;

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

            playerCam.transform.localPosition = new Vector3(x, y, camRelativePosition.z);

            yield return null;
        }

        playerCam.transform.localPosition = camRelativePosition;
    }

    public bool GetDeath()
    {
        return death;
    }

    public void SetDeath(bool _death)
    {
        death = _death;
    }

    public int GetHealth ()
    {
        return playerHealth;
    }

    // GUI 
    void OnGUI()
    {
        if (!hasAuthority)
        {
            return;
        }
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