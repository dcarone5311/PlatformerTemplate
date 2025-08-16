using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        rb.velocity = transform.forward * speed;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            transform.rotation = Quaternion.LookRotation(transform.forward - 2 * Vector3.Dot(transform.forward, normal) * normal);

        }

    }
}
