using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPot : MonoBehaviour
{
    public GameObject IntactPot;
    public GameObject BrokenPot;
    private Collider Collider;
    private float OpacityTimer = 3;
    public Material[] PotMats;
    public MeshRenderer[] Shards;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Shards = BrokenPot.GetComponentsInChildren<MeshRenderer>();
    }
    private void Update()
    {
        OpacityTimer -= Time.deltaTime;
        if (OpacityTimer > 0)
        {
            for (int i = 0; i < PotMats.Length; i++)
            {
                PotMats[i].color = new Color(PotMats[i].color.r, PotMats[i].color.g, PotMats[i].color.b, OpacityTimer/3);
            }

            for (int i = 0; i < Shards.Length; i++)
            {
                Shards[i].materials = PotMats;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Collider.enabled = false;
            BrokenPot.SetActive(true);
            IntactPot.SetActive(false);
            StartCoroutine(Break());
            OpacityTimer = 3;
        }
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(3);
        Destroy(BrokenPot);
    }
}
