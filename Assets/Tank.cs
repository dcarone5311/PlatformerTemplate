using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{

    public float forwardSpeed, reverseSpeed, turretSensitivity, turnSpeed, acceleration;
    public Transform Turret, SpawnPoint;

    

    float currentVelocity;

    public GameObject bulletPrefab;
    protected Rigidbody rb;


    //public RectTransform crosshair;




    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentVelocity =0f;
        rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
           
    }

    protected virtual void FixedUpdate()
    {

    }

    protected void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = SpawnPoint.position;
        bullet.transform.rotation = SpawnPoint.rotation;

        Destroy(bullet, 3f);
    }

}
