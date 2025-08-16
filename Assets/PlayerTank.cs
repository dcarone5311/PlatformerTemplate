using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerTank : MonoBehaviour
{

    public float forwardSpeed, reverseSpeed, turretSensitivity, turnSpeed, acceleration;
    public Transform Turret, SpawnPoint;



    float currentVelocity;

    public GameObject bulletPrefab;
    public GameObject explosion;
    protected Rigidbody rb;


    public RectTransform crosshair;
    public float coolDown;

    float timer;


    // Start is called before the first frame update
    void Start()
    {
        currentVelocity = 0f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        crosshair.position = Input.mousePosition;
        if (timer > coolDown && Input.GetMouseButtonDown(0))
            Shoot();

    }

    void FixedUpdate()
    {

        Vector3 rotate = Vector3.up * Input.GetAxisRaw("Horizontal") * turnSpeed * 100f * Time.fixedDeltaTime;
        rb.rotation = rb.rotation * Quaternion.Euler(rotate);
        Turret.Rotate(-rotate);

        float forward = Input.GetAxisRaw("Vertical");


        float targetVelocity = (forward > 0 ? forwardSpeed : reverseSpeed) * forward;

        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);


        rb.velocity = currentVelocity * transform.forward;

        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        float hitDist = 0;

        if (groundPlane.Raycast(raycast, out hitDist))
        {
            Vector3 rayHitPoint = raycast.GetPoint(hitDist);

            Quaternion targetRotation = Quaternion.LookRotation(rayHitPoint - transform.position);

            Turret.rotation = Quaternion.Slerp(Turret.rotation, targetRotation, turretSensitivity * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Explode();
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = SpawnPoint.position;
        bullet.transform.rotation = SpawnPoint.rotation;
        timer = 0;
        Destroy(bullet, 3f);
    }

    void Explode()
    {
        GameObject explode = Instantiate(explosion);
        explode.transform.position = transform.position;
        float time = explode.GetComponent<ParticleSystem>().main.duration;
        Destroy(explode, time);
        Destroy(gameObject);
    }
}
