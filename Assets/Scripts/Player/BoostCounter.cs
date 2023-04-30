using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoostCounter : MonoBehaviour
{

    TextMeshProUGUI display;
    Movement player;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Movement>();
        if (player.hasPowerup)
        {
            display.text = "Boost Left: " + player.powerupLeft;
        }
        else
        {
            display.text = null;
        }
    }
}
