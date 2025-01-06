using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    public float HP = 5;
    private Material[] OriginalMaterial;
    public Material[] HitFlash;
    public GameObject Mesh;
    // Start is called before the first frame update
    void Awake()
    {
        OriginalMaterial = Mesh.GetComponent<SkinnedMeshRenderer>().materials;
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
        Mesh.GetComponent<SkinnedMeshRenderer>().materials = HitFlash;
        yield return new WaitForSeconds(0.1f);
        Mesh.GetComponent<SkinnedMeshRenderer>().materials = OriginalMaterial;
    }
}
