using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Player Data")] // Makes me able to create a scriptable object within the project
public class PlayerData : ScriptableObject
{
    // Big Script just made for keeping unity inspectors tidy
    // It contains all nescessary public variables, which can be referenced within the "player movement" script

    [Header("Movement")]
    public float WalkSpeed;
    public float SprintSpeed;
    public float SprintLimit;
    public float JumpForce;
    public float JumpUpForce;


}
