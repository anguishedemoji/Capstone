using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimerUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    GameManager gameManager;
    Text text;

    // Update is called once per frame
    void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if(gameManager == null)
            {
                //Game probably hasnt started
                return;
            }
        }
        text.text = gameManager.TimeLeft.ToString("#.00");
    }
}
