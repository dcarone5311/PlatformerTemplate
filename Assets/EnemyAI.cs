using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum States
{
    patrol,
    pursuit,
    attack
}

public class EnemyAI : MonoBehaviour
{
    public bool debug;
    public float forwardSpeed, reverseSpeed, turretSensitivity, turnSpeed, acceleration, radius, coolDown;
    public Transform Turret, SpawnPoint;



    float currentVelocity;

    public GameObject bulletPrefab;
    public GameObject explosion;
    protected Rigidbody rb;



    Vector3 TargetPoint;
    public Transform playerPos;

    public States currentState;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        ChooseRandomPoint();
        currentState = States.patrol;
        //playerPos =  GameObject.FindGameObjectWithTag("Player").transform;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        
 

        switch (currentState)
        {
            case States.patrol:
                if (Vector3.Distance(playerPos.position, transform.position) < radius)
                {
                    currentState = States.pursuit;

                }
                else if (Vector3.Distance(TargetPoint, transform.position) < 0.2f)
                    ChooseRandomPoint();
                return;

            case States.pursuit:
                TargetPoint = playerPos.position;
                timer += Time.deltaTime;
                if (Vector3.Distance(playerPos.position, transform.position) > radius * 2f)
                {
                    currentState = States.patrol;
                    timer = 0f;
                    ChooseRandomPoint();
                }

                if(timer > coolDown)
                {
                    timer = 0f;
                    Shoot();
                }

                return;

            case States.attack:

                return;



        }

    }

    private void FixedUpdate()
    {

        Quaternion targetRotation = Quaternion.LookRotation(TargetPoint - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        

        rb.velocity = forwardSpeed * transform.forward;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        if(collision.gameObject.tag == "Bullet")
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

    void ChooseRandomPoint()
    {

        TargetPoint = new Vector3(Random.value * 8f - 4f, transform.position.y, Random.value * 8f - 4f);

    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetPoint, .1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

}
