using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollower : MonoBehaviour
{
    public float CameraHeight = 20;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Player.transform.position;
        pos.y += CameraHeight;
        pos.z -= CameraHeight*2/3;
        transform.position = pos;
    }
}
