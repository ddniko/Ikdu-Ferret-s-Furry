using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private float timer = 4;
    private bool Hit;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 || Hit)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Pot")
        {
            Hit = true;
        }
    }


}
