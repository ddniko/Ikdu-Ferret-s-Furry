using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public Sprite[] HPSprite;
    private Image UI;
    public PlayerMovement Player;
    void Start()
    {
        UI = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UI.sprite = HPSprite[Player.HP];
    }
}
