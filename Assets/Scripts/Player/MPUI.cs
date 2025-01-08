using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPUI : MonoBehaviour
{
    public Sprite[] MPSprite;
    private Image UI;
    public PlayerMovement Player;
    void Start()
    {
        UI = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UI.sprite = MPSprite[Player.MP-1];
    }
}
