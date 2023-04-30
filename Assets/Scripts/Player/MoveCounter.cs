using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveCounter : MonoBehaviour
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
        display.text = "Time Left: " + player.movesLeft;
    }
}
