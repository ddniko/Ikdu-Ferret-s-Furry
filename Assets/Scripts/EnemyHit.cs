using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    public float HP = 5;
    private Material OriginalMaterial;
    public Material HitFlash;
    // Start is called before the first frame update
    void Awake()
    {
        OriginalMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Debug.Log("Hit");
            HP--;
            StartCoroutine(Hit());
        }
    }
    IEnumerator Hit()
    {
        GetComponent<MeshRenderer>().material = HitFlash;
        yield return new WaitForSeconds(0.1f);
        GetComponent<MeshRenderer>().material = OriginalMaterial;
    }
}
